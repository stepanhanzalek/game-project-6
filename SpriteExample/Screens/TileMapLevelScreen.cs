using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ParticleSystemExample;
using WrongHole.Objects;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class TileMapLevelScreen : WorldWithBordersScreen
    {
        public int CurrentLevel;
        public LevelState _levelState = LevelState.Playing;
        protected List<Hole> _holes;

        protected SortedDictionary<int, Ball> _balls;

        protected Dictionary<int, Ball> _ballsPassive;

        protected IEnumerator<KeyValuePair<int, Ball>> _currBall;

        protected Cue _cue;

        protected FireworkParticleSystem _fireworks;

        protected float _shakingCountdown;

        protected Monochrome _monochrome;

        protected TileMap _tilemap;

        protected Random _random = new Random();

        protected string _tilemapName;

        protected KeyboardState _lastState, _currState;

        protected bool Debug = false;

        public TileMapLevelScreen(int currLvl) : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _tilemapName = $"lvl{currLvl}";
            CurrentLevel = currLvl;
        }

        public TileMapLevelScreen(int currLvl, Monochrome monochrome) : base()
        {
            _monochrome = monochrome;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _tilemapName = $"lvl{currLvl}";
            CurrentLevel = currLvl;
        }

        public enum LevelState
        {
            Playing,
            Success,
            Fail,
            Pause,
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

            if (_monochrome == null) _monochrome = Constants.MONOCHROMES[_random.Next(Constants.MONOCHROMES.Length)];

            _tilemap = _content.Load<TileMap>(Constants.TILEMAP_PATH + this._tilemapName);

            _tilemap.AddBorders(_world);

            _balls = new SortedDictionary<int, Ball>();
            _ballsPassive = new Dictionary<int, Ball>();
            _holes = new List<Hole>();
            _cue = new Cue();

            int ballRadius = (int)(Constants.BALL_RADIUS * _tilemap.Scale);

            foreach (var obj in _tilemap.Objects)
            {
                switch (obj.Data.Type)
                {
                    case "CIRCLE":
                        var circle = ObjectFactory.GetCircleBall(_world, ballRadius, _monochrome.Ball, _monochrome.BallActive, _tilemap.GetPos(obj.Position));
                        _balls.Add(circle.Item1, circle.Item2);
                        break;

                    case "SQUARE":
                        var square = ObjectFactory.GetSquareBall(_world, ballRadius, _monochrome.Ball, _monochrome.BallActive, _tilemap.GetPos(obj.Position));
                        _balls.Add(square.Item1, square.Item2);
                        break;

                    case "PASSIVE_CIRCLE":
                        var passCircle = ObjectFactory.GetCircleBall(_world, ballRadius, _monochrome.BallPassive, _monochrome.BallPassive, _tilemap.GetPos(obj.Position));
                        _ballsPassive.Add(passCircle.Item1, passCircle.Item2);
                        break;

                    case "PASSIVE_SQUARE":
                        var passSquare = ObjectFactory.GetSquareBall(_world, ballRadius, _monochrome.BallPassive, _monochrome.BallPassive, _tilemap.GetPos(obj.Position));
                        _ballsPassive.Add(passSquare.Item1, passSquare.Item2);
                        break;

                    case "HOLE_CIRCLE":
                        _holes.Add(ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _monochrome.Hole, _tilemap.GetPos(obj.Position), obj.Data.Lifespan));
                        break;

                    case "HOLE_SQUARE":
                        _holes.Add(ObjectFactory.GetSquareHole(_world, new Point(ballRadius), _monochrome.Hole, _tilemap.GetPos(obj.Position), obj.Data.Lifespan));
                        break;
                }
            }

            foreach (var hole in _holes) hole.LoadContent(_content);
            foreach (var (_, ball) in _balls) ball.LoadContent(_content);
            foreach (var (_, ball) in _ballsPassive) ball.LoadContent(_content);
            _cue.LoadContent(_content, _monochrome.BallActive);

            _currBall = _balls.GetEnumerator();
            _currBall.MoveNext();

            _currState = _lastState = Keyboard.GetState();

            _fireworks = new FireworkParticleSystem(ScreenManager.Game, 20);
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _lastState = _currState;
            _currState = Keyboard.GetState();

            if (_lastState.IsKeyDown(Keys.Space) && !_currState.IsKeyDown(Keys.Space))
            {
                if (!_currBall.MoveNext() && _currBall.Current.Value == null)
                {
                    _currBall = _balls.GetEnumerator();
                    _currBall.MoveNext();
                };
            }

            if (_levelState == LevelState.Playing)
            {
                var activeBall = _currBall.Current.Value;
                _cue.Update(gameTime, ref activeBall);
            }
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (var (_, ball) in _balls) ball.Update(gameTime);

            foreach (var (_, ball) in _ballsPassive) ball.Update(gameTime);

            List<Hole> removeHoles = new List<Hole>();

            foreach (var hole in _holes)
            {
                if (hole.Colliding && (_balls.ContainsKey(hole.CollisionHash) || _ballsPassive.ContainsKey(hole.CollisionHash)))
                {
                    ResolveCollision(hole, ref removeHoles);

                    if (_balls.Count == 0)
                    {
                        if (_ballsPassive.Count == 0) Win();
                        else Fail();
                    }
                    else _currBall = _balls.GetEnumerator();
                    if (_balls.Count != 0) _currBall.MoveNext();
                }

                hole.Update(gameTime);
            }

            foreach (var hole in removeHoles.ToArray())
            {
                _world.Remove(hole._body);
                _holes.Remove(hole);
            }

            if ((_balls.Count + _ballsPassive.Count != 0) && _holes.Count == 0) Fail();

            _fireworks.Update(gameTime);

            if (_shakingCountdown > 0f) _shakingCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_levelState != LevelState.Playing && _shakingCountdown <= 0f) this.NextLevel();

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public virtual void NextLevel()
        {
            ScreenManager.AddScreen(new NextLevelMenuScreen(this), null);
            ScreenManager.RemoveScreen(this);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            _graphics.Clear(_monochrome.Background);

            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(_random.Next(-1, 1), _random.Next(-1, 1), 0));
            else spriteBatch.Begin();

            _tilemap.Draw(spriteBatch, _monochrome.Wall, ScreenManager.Font, Debug);

            foreach (var hole in _holes) hole.Draw(spriteBatch);

            foreach (var (hash, ball) in _balls)
            {
                if (hash == _currBall.Current.Key) ball.Draw(spriteBatch, true);
                else ball.Draw(spriteBatch, false);
            }

            foreach (var (_, ball) in _ballsPassive) ball.Draw(spriteBatch, false);

            if (_levelState == LevelState.Playing) _cue.Draw(spriteBatch, _currBall.Current.Value == null ? tainicom.Aether.Physics2D.Common.Vector2.Zero : _currBall.Current.Value._body.Position);

            spriteBatch.End();

            _fireworks.Draw(gameTime);
        }

        protected virtual void ResolveCollision(Hole hole, ref List<Hole> removeHoles)
        {
            var toDelete = _balls.ContainsKey(hole.CollisionHash)
                        ? _balls[hole.CollisionHash]
                        : _ballsPassive[hole.CollisionHash];
            if (hole.Shape == toDelete.Shape)
            {
                _fireworks.PlaceFirework(Tools.ToXnaVector(toDelete._body.Position), _monochrome.FireworkWin);

                if (--hole.Lifespan == 0) removeHoles.Add(hole);
            }
            else
            {
                Fail();
                _fireworks.PlaceFirework(Tools.ToXnaVector(toDelete._body.Position), _monochrome.FireworkFail);
            }
            _world.Remove(toDelete._body);
            if (_balls.ContainsKey(hole.CollisionHash)) _balls.Remove(hole.CollisionHash);
            else _ballsPassive.Remove(hole.CollisionHash);
        }

        protected virtual void Win()
        {
            ScreenManager.SoundEffectManager.Win.Play();
            _shakingCountdown = 2f;
            WrongHoleGame.score[CurrentLevel] = true;
            _levelState = LevelState.Success;
        }

        protected virtual void Fail()
        {
            ScreenManager.SoundEffectManager.Explosion.Play();
            _shakingCountdown = 2f;
            if (!WrongHoleGame.score[CurrentLevel]) WrongHoleGame.score[CurrentLevel] = false;
            _levelState = LevelState.Fail;
        }
    }
}
