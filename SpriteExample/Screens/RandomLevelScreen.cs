using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WrongHole.Content;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class RandomLevelScreen : GameScreen
    {
        public bool Active = true;
        private ContentManager _content;

        private PlayerBall playerBall;

        private Hole[] holes;

        private List<Ball> balls;

        private GraphicsDevice GraphicsDevice;

        private SoundEffect hit;

        private SoundEffect win;

        private bool isLast;

        public RandomLevelScreen(Game game, bool isLast = false)
        {
            this.isLast = isLast;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            Random rand = new Random();

            Vector2[] positions = new Vector2[5];

            int i = 0;

            while (i < 5)
            {
                positions[i] = new Vector2((float)rand.NextDouble() * 800 * .75f,
                (float)rand.NextDouble() * 480 * .75f);
                for (int j = 0; j < i; j++) if (CollisionHelper.Collides(32, positions[i], positions[j])) continue;
                i++;
            }

            playerBall = new PlayerBall();
            balls = new List<Ball> {
                new Ball(positions[0]),
                new Ball(positions[1]),
                new Ball(positions[2]),
                new Ball(positions[3]),
                new Ball(positions[4]),
            };

            holes = new Hole[] {
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
            };
            this.isLast = isLast;
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            GraphicsDevice = ScreenManager.GraphicsDevice;

            foreach (var hole in holes) hole.LoadContent(GraphicsDevice, _content, "hole1");
            holes[0].LoadContent(GraphicsDevice, _content, "hole2");
            holes[0].dummy = false;

            playerBall.LoadContent(GraphicsDevice, _content, "ball");
            foreach (var ball in balls) ball.LoadContent(GraphicsDevice, _content, "ball");
            balls.Add(playerBall);

            hit = _content.Load<SoundEffect>("hitHurt");
            win = _content.Load<SoundEffect>("pickupCoin");
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!Active) return;

            foreach (var hole in holes)
            {
                if (playerBall.Bounds.CollidesWith(hole.Bounds) && !hole.dummy) { if (isLast) ScreenManager.Game.Exit(); else ExitScreen(); }
            }

            foreach (var ball in balls) ball.Update(gameTime, GraphicsDevice);

            for (int i = 0; i < balls.Count; i++)
            {
                foreach (var hole in holes) if (
                        balls[i].Bounds.CollidesWith(hole.Bounds)
                        && hole.dummy
                        && balls[i] != playerBall) { win.Play(); balls.Remove(balls[i]); }
                for (int j = i + 1; j < balls.Count; j++)
                {
                    var res = new float[] { balls[i].Velocity.X - balls[j].Velocity.X, balls[i].Velocity.Y - balls[j].Velocity.Y };
                    if (balls[i].CollidesWith(balls[j]) && res[0] * (balls[j].Center.X - balls[i].Center.X) + res[1] * (balls[j].Center.Y - balls[i].Center.Y) >= 0)
                    {
                        // TODO: Handle collisions
                        Vector2 collisionAxis = balls[i].Center - balls[j].Center;
                        collisionAxis.Normalize();

                        float angle = -(float)System.Math.Atan2(balls[i].Center.Y - balls[j].Center.Y, balls[i].Center.X - balls[j].Center.X);

                        Vector2 u0 = Vector2.Transform(balls[i].Velocity, Matrix.CreateRotationZ(angle));
                        Vector2 u1 = Vector2.Transform(balls[j].Velocity, Matrix.CreateRotationZ(angle));

                        Vector2 v0 = new Vector2(
                            2 * balls[j].Radius / (balls[i].Radius + balls[j].Radius) * u1.X,
                            u0.Y
                            );
                        Vector2 v1 = new Vector2(
                            2 * balls[i].Radius / (balls[i].Radius + balls[j].Radius) * u0.X,
                            u1.Y
                            );

                        balls[i].Velocity = Vector2.Transform(v0, Matrix.CreateRotationZ(-angle));
                        balls[j].Velocity = Vector2.Transform(v1, Matrix.CreateRotationZ(-angle));

                        balls[i].IsColliding = balls[j].IsColliding = true;

                        hit.Play();
                    }
                    else if (!balls[i].CollidesWith(balls[j]))
                    {
                        balls[i].IsColliding = balls[j].IsColliding = false;
                    }
                }
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            GraphicsDevice.Clear(Color.LawnGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var hole in holes) hole.Draw(spriteBatch);
            foreach (var ball in balls) ball.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
