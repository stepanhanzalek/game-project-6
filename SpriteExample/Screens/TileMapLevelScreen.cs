using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleSystemExample;
using WrongHole.Objects;
using WrongHole.Screens.GameScreens;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class TileMapLevelScreen : WorldWithBordersScreen
    {
        public int CurrLevel;
        private List<Hole> holes;

        private SortedDictionary<int, Ball> balls;

        private IEnumerator<KeyValuePair<int, Ball>> currBall;

        private SoundEffect hit;

        private SoundEffect win;

        private SoundEffect explosion;

        private SpriteFont Font;

        private PlayerBall PlayerBall;

        private FireworkParticleSystem _fireworks;

        private float _shakingCountdown;

        private Color[] _colorPallete;

        private TileMap _tilemap;

        private Random random = new Random();

        private string _tilemapName;

        private KeyboardState lastState, currState;

        private GameState state = GameState.Playing;

        public TileMapLevelScreen(int currLvl) : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _tilemapName = $"lvl{currLvl}";
            CurrLevel = currLvl;
        }

        private enum GameState
        {
            Playing,
            Success,
            Fail,
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

            _colorPallete = Constants.MONOCHROMES[random.Next(Constants.MONOCHROMES.Length)];

            hit = _content.Load<SoundEffect>("hitHurt");
            win = _content.Load<SoundEffect>("pickupCoin");
            Font = _content.Load<SpriteFont>("GNUTypewriter");
            explosion = _content.Load<SoundEffect>("explosion");
            _tilemap = _content.Load<TileMap>(this._tilemapName);

            _tilemap.AddBorders(_world);

            balls = new SortedDictionary<int, Ball>();
            holes = new List<Hole>();
            PlayerBall = new PlayerBall();

            int ballRadius = (int)(Constants.BALL_RADIUS * _tilemap.Scale);

            foreach (var obj in _tilemap.Objects)
            {
                switch (obj.Data.Type)
                {
                    case "CIRCLE":
                        var circle = ObjectFactory.GetCircleBall(_world, ballRadius, _colorPallete[3], _colorPallete[4], _tilemap.GetPos(obj.Position));
                        balls.Add(circle.Item1, circle.Item2);
                        break;

                    case "SQUARE":
                        var square = ObjectFactory.GetSquareBall(_world, ballRadius, _colorPallete[3], _colorPallete[4], _tilemap.GetPos(obj.Position));
                        balls.Add(square.Item1, square.Item2);
                        break;

                    case "HOLE_CIRCLE":
                        holes.Add(ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _colorPallete[1], _tilemap.GetPos(obj.Position), obj.Data.Lifespan));
                        break;

                    case "HOLE_SQUARE":
                        holes.Add(ObjectFactory.GetSquareHole(_world, new Point(ballRadius), _colorPallete[1], _tilemap.GetPos(obj.Position), obj.Data.Lifespan));
                        break;
                }
            }

            foreach (var hole in holes) hole.LoadContent(_content);
            foreach (var (_, ball) in balls) ball.LoadContent(_content);
            PlayerBall.LoadContent(_content, _colorPallete[4]);

            currBall = balls.GetEnumerator();
            currBall.MoveNext();

            currState = lastState = Keyboard.GetState();

            _fireworks = new FireworkParticleSystem(ScreenManager.Game, 20);
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
            if (otherScreenHasFocus) return;

            lastState = currState;
            currState = Keyboard.GetState();

            if (lastState.IsKeyDown(Keys.Space) && !currState.IsKeyDown(Keys.Space))
            {
                if (!currBall.MoveNext() && currBall.Current.Value == null)
                {
                    currBall = balls.GetEnumerator();
                    currBall.MoveNext();
                };
            }

            foreach (var (_, ball) in balls) ball.Update(gameTime);

            List<Hole> removeHoles = new List<Hole>();

            foreach (var hole in holes)
            {
                if (hole.Colliding && balls.ContainsKey(hole.CollisionHash))
                {
                    var toDelete = balls[hole.CollisionHash];
                    if (hole.Shape == toDelete.Shape)
                    {
                        _fireworks.PlaceFirework(Tools.ToXnaVector(toDelete._body.Position), _colorPallete[4]);

                        if (--hole.Lifespan == 0) removeHoles.Add(hole);
                    }
                    else
                    {
                        explosion.Play();
                        _shakingCountdown = 2f;
                        if (!WrongHoleGame.score[CurrLevel]) WrongHoleGame.score[CurrLevel] = false;
                        state = GameState.Fail;
                        _fireworks.PlaceFirework(Tools.ToXnaVector(toDelete._body.Position), _colorPallete[0]);
                    }
                    _world.Remove(toDelete._body);
                    balls.Remove(hole.CollisionHash);

                    if (balls.Values.Count == 0)
                    {
                        win.Play();
                        _shakingCountdown = 2f;
                        WrongHoleGame.score[CurrLevel] = true;
                        state = GameState.Success;
                    }
                    else currBall = balls.GetEnumerator();
                    if (balls.Count != 0) currBall.MoveNext();
                }

                hole.Update(gameTime);
            }

            foreach (var hole in removeHoles.ToArray())
            {
                _world.Remove(hole._body);
                holes.Remove(hole);
            }

            _fireworks.Update(gameTime);

            if (_shakingCountdown > 0f) _shakingCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state != GameState.Playing && _shakingCountdown <= 0f)
            {
                switch (state)
                {
                    case GameState.Success:
                        this.NextLevel();
                        break;

                    case GameState.Fail:
                        this.NextLevel();
                        break;
                }
            }

            if (state == GameState.Playing)
            {
                var activeBall = currBall.Current.Value;
                PlayerBall.Update(gameTime, ref activeBall);
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public void NextLevel()
        {
            ScreenManager.AddScreen(new NextLevelMenuScreen(this), null);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            _graphics.Clear(_colorPallete[2]);

            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(random.Next(-1, 1), random.Next(-1, 1), 0));
            else spriteBatch.Begin();

            DrawBoard(ref spriteBatch, _colorPallete[3], _colorPallete[1]);

            foreach (var hole in holes) hole.Draw(spriteBatch);
            foreach (var (hash, ball) in balls)
            {
                if (hash == currBall.Current.Key) ball.Draw(spriteBatch, true);
                else ball.Draw(spriteBatch, false);
            }

            if (state == GameState.Playing) PlayerBall.Draw(spriteBatch, currBall.Current.Value == null ? tainicom.Aether.Physics2D.Common.Vector2.Zero : currBall.Current.Value._body.Position);

            spriteBatch.End();

            _fireworks.Draw(gameTime);
        }

        protected override void DrawBoard(ref SpriteBatch spriteBatch, Color lighter, Color darker)
        {
            _tilemap.Draw(spriteBatch, lighter, Font);
        }
    }
}
