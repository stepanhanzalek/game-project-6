using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using WrongHole.Objects;
using WrongHole.Screens;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole
{
    public class EndlessLevelScreen : TileMapLevelScreen
    {
        protected PolygonShape _tileMapBoundingBox;

        protected int _score = 0;

        public EndlessLevelScreen() : base(0)
        {
        }

        public override void Activate()
        {
            _tilemapName = "lvl_endless";

            base.Activate();

            _tileMapBoundingBox = ObjectFactory.GetTileMapBox(_tilemap.Borders);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (_levelState != LevelState.Playing) return;

            base.HandleInput(gameTime, input);

            if (_lastState.IsKeyDown(Keys.Escape) && !_currState.IsKeyDown(Keys.Escape)) Pause();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (_levelState != LevelState.Playing) return;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            _graphics.Clear(_monochrome.Background);

            if (_shakingCountdown > 0f) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(_random.Next(-1, 1), _random.Next(-1, 1), 0));
            else spriteBatch.Begin();

            _tilemap.Draw(spriteBatch, _monochrome.Wall, ScreenManager.Font, Debug);

            DrawScore(spriteBatch);

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

        public override void NextLevel()
        {
            if (_levelState != LevelState.Playing) return;

            ScreenManager.AddScreen(new PauseLevelScreen(this), null);
        }

        protected override void ResolveCollision(Hole hole, ref List<Hole> removeHoles)
        {
            var toDelete = _balls.ContainsKey(hole.CollisionHash)
                        ? _balls[hole.CollisionHash]
                        : _ballsPassive[hole.CollisionHash];
            if (hole.Shape == toDelete.Shape)
            {
                _fireworks.PlaceFirework(Tools.ToXnaVector(toDelete._body.Position), _monochrome.FireworkWin);

                GenerateBall();

                ++_score;

                if (_score >= WrongHoleGame.hiScore) WrongHoleGame.hiScore = _score;
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

        protected void GenerateBall()
        {
            var pos = Tools.ToAetherVector(RandomHelper.RandomScreenPosition());
            var tilePos = new Transform(_tilemap.GetPos((0, 0)), 0);
            while (!_tileMapBoundingBox.TestPoint(ref tilePos, ref pos)) pos = Tools.ToAetherVector(RandomHelper.RandomScreenPosition());

            var circle = ObjectFactory.GetCircleBall(_world, (int)(Constants.BALL_RADIUS * _tilemap.Scale), _monochrome.Ball, _monochrome.BallActive, pos);
            _balls.Add(circle.Item1, circle.Item2);
            circle.Item2.LoadContent(_content);
        }

        protected void DrawScore(SpriteBatch spriteBatch)
        {
            var scoreText = _score == WrongHoleGame.hiScore
                ? $"score - {_score}"
                : $"score - {_score} < {WrongHoleGame.hiScore}";
            (string Text, (float X, float Y) Position) Text = (scoreText, (Constants.GAME_CENTER.X, Constants.GAME_CENTER.Y));
            spriteBatch.DrawString(
                ScreenManager.Font,
                Text.Text,
                Constants.GAME_CENTER - ScreenManager.Font.MeasureString(Text.Text) / 2,
                _monochrome.Text);
        }

        protected override void Win()
        {
        }

        protected override void Fail()
        {
            ScreenManager.SoundEffectManager.Explosion.Play();
            _shakingCountdown = 2f;
            if (!WrongHoleGame.score[CurrentLevel]) WrongHoleGame.score[CurrentLevel] = false;
            _levelState = LevelState.Fail;
        }

        protected void Pause()
        {
            _levelState = LevelState.Pause;
            ScreenManager.AddScreen(new PauseLevelScreen(this), null);
        }
    }
}
