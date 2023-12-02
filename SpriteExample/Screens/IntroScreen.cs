using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystemExample;
using WrongHole.Objects;
using WrongHole.Screens.GameScreens;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class IntroScreen : WorldWithBordersScreen
    {
        private Hole holeClassic;
        private Hole holeEndless;
        private Hole holeExit;

        private Ball ball;

        private SoundEffect hit;

        private SoundEffect win;

        private SpriteFont Font;

        private SpriteFont FontTitle;

        private PlayerBall PlayerBall;

        private FireworkParticleSystem _fireworks;

        private float _shakingCountdown;

        private ExitState exit = ExitState.Idle;

        private Color[] _colorPallete;

        private TileMap _tilemap;

        private Random random = new Random();

        private string _tilemapName;

        private Queue<double> titleCurve = new Queue<double>(Constants.TITLE_CURVE);

        private double titleCountdown = 0f;

        private string title = "Wr ng H0le";

        public IntroScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _tilemapName = "intro";
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

            _colorPallete = Constants.MONOCHROMES[random.Next(Constants.MONOCHROMES.Length)];

            hit = _content.Load<SoundEffect>("hitHurt");
            win = _content.Load<SoundEffect>("pickupCoin");
            Font = _content.Load<SpriteFont>("GNUTypewriter");
            FontTitle = _content.Load<SpriteFont>("GNUTypewriterTitle");
            _tilemap = _content.Load<TileMap>(this._tilemapName);

            _tilemap.AddBorders(_world);

            PlayerBall = new PlayerBall();

            int ballRadius = (int)(Constants.BALL_RADIUS * _tilemap.Scale);

            foreach (var obj in _tilemap.Objects)
            {
                switch (obj.Data.Type)
                {
                    case "CIRCLE":
                        ball = ObjectFactory.GetCircleBall(_world, ballRadius, _colorPallete[3], _colorPallete[4], _tilemap.GetPos(obj.Position)).Item2;
                        break;

                    case "HOLE_CLASSIC":
                        holeClassic = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _colorPallete[1], _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;

                    /*case "HOLE_ENDLESS":
                        holeEndless = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _colorPallete[1], _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;*/

                    case "HOLE_EXIT":
                        holeExit = ObjectFactory.GetCircleHole(_world, new Point(ballRadius), _colorPallete[1], _tilemap.GetPos(obj.Position), obj.Data.Lifespan);
                        break;
                }
            }

            ball.LoadContent(_content);
            holeClassic.LoadContent(_content);
            //holeEndless.LoadContent(_content);
            holeExit.LoadContent(_content);
            PlayerBall.LoadContent(_content, _colorPallete[4]);

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

            ball.Update(gameTime);

            if (holeClassic.Colliding)
            {
                _fireworks.PlaceFirework(Tools.ToXnaVector(ball._body.Position), _colorPallete[4]);
                _world.Remove(ball._body);
                win.Play();
                _shakingCountdown = 2f;
                exit = ExitState.Classic;
            }

            /*if (holeEndless.Colliding)
            {
                //_fireworks.PlaceFirework(Tools.ToXnaVector(ball._body.Position), _colorPallete[4]);
                //_world.Remove(ball._body);
                //win.Play();
                //_shakingCountdown = 2f;
                //exit = ExitState.Endless;
            }*/

            if (holeExit.Colliding)
            {
                _fireworks.PlaceFirework(Tools.ToXnaVector(ball._body.Position), _colorPallete[0]);
                _world.Remove(ball._body);
                win.Play();
                _shakingCountdown = 2f;
                exit = ExitState.Exit;
            }

            holeClassic.Update(gameTime);
            //holeEndless.Update(gameTime);
            holeExit.Update(gameTime);
            _fireworks.Update(gameTime);

            if (_shakingCountdown > 0f) _shakingCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (exit != ExitState.Idle && _shakingCountdown <= 0f)
            {
                switch (exit)
                {
                    case ExitState.Classic:
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new LevelSelectionMenuScreen(), null);
                        break;

                    case ExitState.Exit:
                        ExitScreen();
                        break;
                }
            }

            if (exit == ExitState.Idle) PlayerBall.Update(gameTime, ref ball);

            UpdateTitle(gameTime);

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public void UpdateTitle(GameTime gameTime)
        {
            if (titleCountdown <= 0f)
            {
                title = $"{RandomHelper.RandomString(2)} {RandomHelper.RandomString(2)} {RandomHelper.RandomString(4)}";
                if (titleCurve.Count == 0) titleCurve = new Queue<double>(Constants.TITLE_CURVE);
                titleCountdown = titleCurve.Dequeue();
                if (titleCountdown == 5000f) title = "Wr ng H0le";
            }
            titleCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            _graphics.Clear(_colorPallete[2]);

            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(random.Next(-1, 1), random.Next(-1, 1), 0));
            else spriteBatch.Begin();

            DrawBoard(ref spriteBatch, _colorPallete[3], _colorPallete[1]);

            if (exit != ExitState.Classic) holeClassic.Draw(spriteBatch);
            //if (exit != ExitState.Endless) holeEndless.Draw(spriteBatch);
            if (exit != ExitState.Exit) holeExit.Draw(spriteBatch);
            if (exit == ExitState.Idle) ball.Draw(spriteBatch, true);

            if (exit == ExitState.Idle) PlayerBall.Draw(spriteBatch, ball._body.Position);

            spriteBatch.DrawString(FontTitle, title, new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 4) - FontTitle.MeasureString(title) / 2, _colorPallete[4]);

            spriteBatch.DrawString(Font, "Classic", new Vector2(Constants.GAME_WIDTH / 4, Constants.GAME_HEIGHT * .7f - 35) - Font.MeasureString("Classic") / 2, _colorPallete[4]);
            spriteBatch.DrawString(Font, "Endless", new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT * .7f - 35) - Font.MeasureString("Endless") / 2, _colorPallete[4]);
            spriteBatch.DrawString(Font, "Exit", new Vector2(Constants.GAME_WIDTH * 3 / 4, Constants.GAME_HEIGHT * .7f - 35) - Font.MeasureString("Exit") / 2, _colorPallete[4]);

            spriteBatch.End();

            _fireworks.Draw(gameTime);
        }

        protected override void DrawBoard(ref SpriteBatch spriteBatch, Color lighter, Color darker)
        {
            _tilemap.Draw(spriteBatch, lighter);
        }
    }
}
