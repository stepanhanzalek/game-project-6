using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    public class SplashScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _font;
        private TimeSpan _displayTime;
        private Color[] _colorPallete;

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _font = _content.Load<SpriteFont>("GNUTypewriter");
            _colorPallete = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];
            _displayTime = TimeSpan.FromSeconds(5);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;

            if (_displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(_colorPallete[2]);
            ScreenManager.SpriteBatch.Begin();
            var pos = Constants.GAME_CENTER - _font.MeasureString("Steve Hanzalek\n   presents") / 2;
            ScreenManager.SpriteBatch.DrawString(_font, "Steve Hanzalek\n   presents", pos, _colorPallete[3]);
            ScreenManager.SpriteBatch.End();
        }
    }
}
