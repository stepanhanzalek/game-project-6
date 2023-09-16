using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteExample.Content;

namespace SpriteExample
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class SpriteExampleGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SlimeGhostSprite slimeGhost;

        private Texture2D atlas;

        private SpriteFont bangers;
        private SpriteFont bangersSmall;

        private PlayerBall playerBall;

        private Vector2 textSize;

        private Hole[] holes;

        private List<Ball> balls;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public SpriteExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            slimeGhost = new SlimeGhostSprite();

            playerBall = new PlayerBall();
            balls = new List<Ball> {
                new Ball(),
                new Ball(),
                new Ball(),
                new Ball(),
                new Ball(),
            };

            holes = new Hole[] {
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var hole in holes) hole.LoadContent(GraphicsDevice, Content, "hole1");
            holes[0].LoadContent(GraphicsDevice, Content, "hole2", new Vector2(490, 240));
            holes[0].dummy = false;
            // TODO: use this.Content to load your game content here
            slimeGhost.LoadContent(Content);

            atlas = Content.Load<Texture2D>("colored_packed");

            playerBall.LoadContent(GraphicsDevice, Content, "ball");
            foreach (var ball in balls) ball.LoadContent(GraphicsDevice, Content, "ball");
            balls.Add(playerBall);

            bangers = Content.Load<SpriteFont>("bangers");
            bangersSmall = Content.Load<SpriteFont>("bangers1");
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            foreach (var hole in holes)
            {
                if (playerBall.Bounds.CollidesWith(hole.Bounds) && !hole.dummy) Exit();
            }

            foreach (var ball in balls) ball.Update(gameTime, GraphicsDevice);

            for (int i = 0; i < balls.Count; i++)
            {
                foreach (var hole in holes) if (
                        balls[i].Bounds.CollidesWith(hole.Bounds)
                        && hole.dummy
                        && balls[i] != playerBall) balls.Remove(balls[i]);
                for (int j = i + 1; j < balls.Count; j++)
                {
                    //var res = [this.vx - otherParticule.vx, this.vy - otherParticule.vy];
                    var res = new float[] { balls[i].Velocity.X - balls[j].Velocity.X, balls[i].Velocity.Y - balls[j].Velocity.Y };
                    //res[0] *(otherParticule.x - this.x) + res[1] * (otherParticule.y - this.y) >= 0
                    if (balls[i].CollidesWith(balls[j]) && (res[0] * (balls[j].Center.X - balls[i].Center.X) + res[1] * (balls[j].Center.Y - balls[i].Center.Y) >= 0))
                    {
                        // TODO: Handle collisions
                        Vector2 collisionAxis = balls[i].Center - balls[j].Center;
                        collisionAxis.Normalize();

                        float angle = -(float)System.Math.Atan2(balls[i].Center.Y - balls[j].Center.Y, balls[i].Center.X - balls[j].Center.X);

                        Vector2 u0 = Vector2.Transform(balls[i].Velocity, Matrix.CreateRotationZ(angle));
                        Vector2 u1 = Vector2.Transform(balls[j].Velocity, Matrix.CreateRotationZ(angle));

                        Vector2 v0 = new Vector2(
                            (2 * balls[j].Radius) / (balls[i].Radius + balls[j].Radius) * u1.X,
                            u0.Y
                            );
                        Vector2 v1 = new Vector2(
                            (2 * balls[i].Radius) / (balls[i].Radius + balls[j].Radius) * u0.X,
                            u1.Y
                            );

                        balls[i].Velocity = Vector2.Transform(v0, Matrix.CreateRotationZ(-angle));
                        balls[j].Velocity = Vector2.Transform(v1, Matrix.CreateRotationZ(-angle));

                        balls[i].IsColliding = balls[j].IsColliding = true;
                    }
                    else if (!balls[i].CollidesWith(balls[j]))
                    {
                        balls[i].IsColliding = balls[j].IsColliding = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            textSize = bangers.MeasureString("WRONG H  LEs");

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var hole in holes) hole.Draw(spriteBatch);
            foreach (var ball in balls) ball.Draw(spriteBatch);
            spriteBatch.DrawString(
                bangers, "WRONG H  LEs",
                new Vector2(
                    GraphicsDevice.Viewport.Width / 2,
                    GraphicsDevice.Viewport.Height / 2)
                - textSize / 2,
                Color.White);
            spriteBatch.DrawString(
                bangersSmall, "Left click and drag to set the direction and speed of the ball. Find the right hole to exit.",
                new Vector2(0,
                    GraphicsDevice.Viewport.Height - 25),
                Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
