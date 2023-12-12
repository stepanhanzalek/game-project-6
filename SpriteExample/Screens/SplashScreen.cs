using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole.Screens
{
    public class SplashScreen : GameScreen
    {
        private ContentManager _content;

        private Monochrome _monochrome;

        private double _effectCountdown;

        private string _text;

        private Queue<int> _effectQueue;

        private double _effectDuration;

        public override void Activate()
        {
            base.Activate();

            _text = "Steve Hanzalek\n   presents";

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _monochrome = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];

            var rnd = new Random();
            _effectQueue = new Queue<int>(
                Enumerable.Range(0, _text.Length)
                    .Where(x => x != 14) // Dirty hack to discard the newline...
                    .OrderBy(x => rnd.Next())
                    .ToArray());

            _effectDuration = 500f;

            _effectCountdown = 3000f;
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _effectCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_effectCountdown <= 0f && _effectQueue.Count != 0)
            {
                _text = new StringBuilder(_text) { [_effectQueue.Dequeue()] = ' ' }.ToString();
                _effectCountdown = (_effectDuration /= 1.15f);
                if (string.IsNullOrWhiteSpace(_text)) ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(_monochrome.Background);
            ScreenManager.SpriteBatch.Begin();
            var pos = Constants.GAME_CENTER - ScreenManager.Font.MeasureString("Steve Hanzalek\n   presents") / 2;
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, _text, pos, _monochrome.TextLight);
            ScreenManager.SpriteBatch.End();
        }
    }
}
