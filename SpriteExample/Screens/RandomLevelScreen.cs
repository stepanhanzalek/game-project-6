using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystemExample;
using WrongHole.Objects;
using WrongHole.Screens.GameScreens;
using WrongHole.StateManagement;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class RandomLevelScreen : WorldWithBordersScreen
    {
        //private PlayerBall playerBall;

        private List<Hole> holes;

        private Dictionary<int, Ball> balls;

        private SoundEffect hit;

        private SoundEffect win;

        private SpriteFont bangers;

        private bool isLast;

        private int PlayerBallHash;
        private Hole GoodHole;

        private FireworkParticleSystem _fireworks;

        private float _shakingCountdown;
        private Random random = new Random();

        private bool exit = false;

        private Color[] _colorPallete;

        public RandomLevelScreen(Game game, int seed, bool isLast = false)
        {
            this.isLast = isLast;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            Random rand = new Random(seed);

            Vector2[] positions = new Vector2[5];

            this.isLast = isLast;

            _fireworks = new FireworkParticleSystem(game, 20);

            _colorPallete = Constants.MONOCHROMES[random.Next(Constants.MONOCHROMES.Length)];
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
            base.Activate();

            hit = _content.Load<SoundEffect>("hitHurt");
            win = _content.Load<SoundEffect>("pickupCoin");
            bangers = _content.Load<SpriteFont>("bangers1");

            balls = new Dictionary<int, Ball>();

            holes = new List<Hole>();
            for (int i = 0; i < random.Next(0, 5); ++i) holes.Add(ObjectFactory.GetHole(_world, random, new Point(Constants.BALL_RADIUS, Constants.BALL_RADIUS / 2), _colorPallete[0], true));
            foreach (var hole in holes) hole.LoadContent(_content);
            GoodHole = ObjectFactory.GetHole(_world, random, new Point(Constants.BALL_RADIUS, Constants.BALL_RADIUS / 2), _colorPallete[0], true);
            GoodHole.LoadContent(_content);

            var playerBall = ObjectFactory.GetPlayerBall(_world, random, Constants.BALL_RADIUS, _colorPallete[4], randomPos: true);
            balls.Add(playerBall.Item1, playerBall.Item2);
            PlayerBallHash = playerBall.Item1;

            for (int j = 0; j < random.Next(0, 5); j++)
            {
                var ball = ObjectFactory.GetBall(_world, random, Constants.BALL_RADIUS, _colorPallete[3], randomPos: true);
                balls.Add(ball.Item1, ball.Item2);
            }

            foreach (var (_, ball) in balls) ball.LoadContent(_content);
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
            foreach (var (_, ball) in balls)
            {
                ball.Update(gameTime);
            }
            foreach (var hole in holes)
            {
                if (hole.Colliding && hole.CollisionHash != PlayerBallHash && balls.ContainsKey(hole.CollisionHash))
                {
                    var toDelete = balls[hole.CollisionHash];
                    _fireworks.PlaceFirework(new Microsoft.Xna.Framework.Vector2(toDelete._body.Position.X, toDelete._body.Position.Y), Color.Red);
                    _world.Remove(toDelete._body);
                    balls.Remove(hole.CollisionHash);
                }
                hole.Update(gameTime);
            }
            if (GoodHole.Colliding && balls.ContainsKey(GoodHole.CollisionHash))
            {
                var toDelete = balls[GoodHole.CollisionHash];
                _fireworks.PlaceFirework(new Microsoft.Xna.Framework.Vector2(toDelete._body.Position.X, toDelete._body.Position.Y), Color.Green);
                _world.Remove(toDelete._body);
                balls.Remove(GoodHole.CollisionHash);
                _shakingCountdown = 1f;
                win.Play();
                if (GoodHole.CollisionHash == PlayerBallHash)
                {
                    _shakingCountdown = 2f;
                    exit = true;
                }
            }

            _fireworks.Update(gameTime);

            if (_shakingCountdown > 0f) _shakingCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (exit && _shakingCountdown <= 0f) ExitScreen();

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            var textSize = bangers.MeasureString("just try to get your ball to the right hole");
            var textSize2 = bangers.MeasureString("the other balls might help you");

            _graphics.Clear(_colorPallete[2]);

            // TODO: Add your drawing code here
            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(random.Next(-1, 1), random.Next(-1, 1), 0));
            else spriteBatch.Begin();
            DrawBoard(ref spriteBatch, _colorPallete[3], _colorPallete[1]);
            foreach (var hole in holes) hole.Draw(spriteBatch);
            GoodHole.Draw(spriteBatch);
            foreach (var (_, ball) in balls) ball.Draw(spriteBatch);
            spriteBatch.DrawString(
                bangers, "just try to get your ball to the right hole",
                new Microsoft.Xna.Framework.Vector2(
                    (Constants.GAME_WIDTH - textSize.X) / 2,
                    3 * textSize.Y),
                _colorPallete[4]);
            spriteBatch.DrawString(
                bangers, "the other balls might help you",
                new Microsoft.Xna.Framework.Vector2(
                    (Constants.GAME_WIDTH - textSize2.X) / 2,
                    Constants.GAME_HEIGHT - 4 * textSize2.Y),
                _colorPallete[4]);
            spriteBatch.End();
            _fireworks.Draw(gameTime);
        }
    }
}
