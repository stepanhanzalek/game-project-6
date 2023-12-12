using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystemExample;
using WrongHole.Objects;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class IntroScreen : WorldWithBordersScreen
    {
        private Hole _holeClassic;
        private Hole _holeEndless;
        private Hole _holeExit;

        private Ball _ball;

        private SpriteFont FontTitle;

        private Cue _cue;

        private FireworkParticleSystem _fireworks;

        private float _shakingCountdown;

        private ExitState _exitState = ExitState.Idle;

        private Monochrome _monochrome;

        private TileMap _tilemap;

        private string _tilemapName;

        private Random _random;

        private Queue<double> _titleCurve;

        private double _titleCountdown;

        private string _title;

        public IntroScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _tilemapName = "intro";
            _random = new Random();
            _title = "Wr ng H0le";
            _titleCountdown = 0f;
            _exitState = ExitState.Idle;
            _titleCurve = new Queue<double>(Constants.TITLE_CURVE);
        }

        private enum ExitState
        {
            Classic,
            Endless,
            Exit,
            Idle,
        }

        public override void Activate()
        {
            base.Activate();

            _monochrome = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];

            FontTitle = _content.Load<SpriteFont>("GNUTypewriterTitle");
            _tilemap = _content.Load<TileMap>(Constants.TILEMAP_PATH + this._tilemapName);

            _tilemap.AddBorders(_world);

            _cue = new Cue();

            int ballRadius = (int)(Constants.BALL_RADIUS * _tilemap.Scale);

            foreach (var obj in _tilemap.Objects)
            {
                switch (obj.Data.Type)
                {
                    case "CIRCLE":
                        _ball = ObjectFactory.GetCircleBall(_world, ballRadius, _monochrome.Ball, _monochrome.BallActive, _tilemap.GetPos(obj.Position)).Item2;
                        break;

                    case "HOLE_CLASSIC":
                        _holeClassic = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _monochrome.Hole, _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;

                    case "HOLE_ENDLESS":
                        _holeEndless = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _monochrome.Hole, _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;

                    case "HOLE_EXIT":
                        _holeExit = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _monochrome.Hole, _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;
                }
            }

            _ball.LoadContent(_content);
            _holeClassic.LoadContent(_content);
            _holeEndless.LoadContent(_content);
            _holeExit.LoadContent(_content);
            _cue.LoadContent(_content, _monochrome.BallActive);

            _fireworks = new FireworkParticleSystem(ScreenManager.Game, 20);
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (_exitState == ExitState.Idle) _cue.Update(gameTime, ref _ball);

            base.HandleInput(gameTime, input);
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _ball.Update(gameTime);

            if (_holeClassic.Colliding)
            {
                InitiateClassic(Tools.ToXnaVector(_ball._body.Position));
                _world.Remove(_ball._body);
            }

            if (_holeEndless.Colliding)
            {
                InitiateEndless(Tools.ToXnaVector(_ball._body.Position));
                _world.Remove(_ball._body);
            }

            if (_holeExit.Colliding)
            {
                InitiateExit(Tools.ToXnaVector(_ball._body.Position));
                _world.Remove(_ball._body);
            }

            if (_shakingCountdown > 0f) _shakingCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_exitState != ExitState.Idle && _shakingCountdown <= 0f)
            {
                switch (_exitState)
                {
                    case ExitState.Classic:
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new LevelSelectionMenuScreen(), null);
                        break;

                    case ExitState.Endless:
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new EndlessLevelScreen(), null);
                        break;

                    case ExitState.Exit:
                        ExitScreen();
                        break;
                }
            }

            _holeClassic.Update(gameTime);
            _holeEndless.Update(gameTime);
            _holeExit.Update(gameTime);
            _fireworks.Update(gameTime);
            UpdateTitle(gameTime);

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public void InitiateExit(Vector2 fireworkPosition)
        {
            _fireworks.PlaceFirework(fireworkPosition, _monochrome.FireworkFail);
            ScreenManager.SoundEffectManager.Explosion.Play();
            _shakingCountdown = 2f;
            _exitState = ExitState.Exit;
        }

        public void InitiateClassic(Vector2 fireworkPosition)
        {
            _fireworks.PlaceFirework(fireworkPosition, _monochrome.FireworkWin);
            ScreenManager.SoundEffectManager.Win.Play();
            _shakingCountdown = 2f;
            _exitState = ExitState.Classic;
        }

        public void InitiateEndless(Vector2 fireworkPosition)
        {
            _fireworks.PlaceFirework(fireworkPosition, _monochrome.FireworkWin);
            ScreenManager.SoundEffectManager.Win.Play();
            _shakingCountdown = 2f;
            _exitState = ExitState.Endless;
        }

        public void UpdateTitle(GameTime gameTime)
        {
            if (_titleCountdown <= 0f)
            {
                _title = $"{RandomHelper.RandomString(2)} {RandomHelper.RandomString(2)} {RandomHelper.RandomString(4)}";
                if (_titleCurve.Count == 0) _titleCurve = new Queue<double>(Constants.TITLE_CURVE);
                _titleCountdown = _titleCurve.Dequeue();
                if (_titleCountdown == 5000f) _title = "Wr ng H0le";
            }
            _titleCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(_random.Next(-1, 1), _random.Next(-1, 1), 0));
            else spriteBatch.Begin();

            DrawTileMap(_monochrome.Background);

            if (_exitState != ExitState.Classic) _holeClassic.Draw(spriteBatch);
            if (_exitState != ExitState.Endless) _holeEndless.Draw(spriteBatch);
            if (_exitState != ExitState.Exit) _holeExit.Draw(spriteBatch);
            if (_exitState == ExitState.Idle) _ball.Draw(spriteBatch, true);

            if (_exitState == ExitState.Idle) _cue.Draw(spriteBatch, _ball._body.Position);

            DrawText(FontTitle, spriteBatch, _title, .5f, .25f);
            DrawText(ScreenManager.Font, spriteBatch, "Classic", .25f, .7f, -35);
            DrawText(ScreenManager.Font, spriteBatch, "Endless", .5f, .7f, -35);
            DrawText(ScreenManager.Font, spriteBatch, "Exit", .75f, .7f, -35);

            spriteBatch.End();

            _fireworks.Draw(gameTime);
        }

        protected void DrawText(SpriteFont font, SpriteBatch spriteBatch, string text, float xMultiplier, float yMultiplier, float yOffset = 0f)
        {
            spriteBatch.DrawString(
                font,
                text,
                new Vector2(
                    Constants.GAME_WIDTH * xMultiplier,
                    Constants.GAME_HEIGHT * yMultiplier + yOffset)
                        - font.MeasureString(text) / 2,
                _monochrome.TextLight);
        }

        protected void DrawTileMap(Color color)
        {
            _tilemap.Draw(ScreenManager.SpriteBatch, color);
        }
    }
}
