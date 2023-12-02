using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    public class NextLevelMenuScreen : GameScreen
    {
        protected List<Button> _buttons;

        protected ContentManager _content;

        protected GraphicsDevice _graphics;

        protected MouseState _mouseStateLast;

        protected Color[] _colorPallete;

        protected TileMapLevelScreen _currLvl;

        public NextLevelMenuScreen(TileMapLevelScreen currentLevel)
        {
            _currLvl = currentLevel;
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _graphics = ScreenManager.GraphicsDevice;

            _buttons = new List<Button>();

            _colorPallete = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];

            var font = _content.Load<SpriteFont>("GNUTypewriter");

            var nextLvl = WrongHoleGame.score[this._currLvl.CurrLevel] && this._currLvl.CurrLevel + 1 < WrongHoleGame.score.Length;

            var buttonTexts =
                nextLvl
                    ? new string[] { "Replay", "Next Level", "Menu" }
                    : new string[] { "Replay", "Menu" };
            var buttonCenters = Tools.AlignButtonsHorizontally(buttonTexts, font);

            var buttonReplay = new TextButton(
                _content,
                buttonCenters[0],
                buttonTexts[0],
                "square",
                font,
                _colorPallete[0],
                _colorPallete[1],
                _colorPallete[3]);
            buttonReplay.ButtonEventHandler += ReplayLevel;
            _buttons.Add(buttonReplay);

            if (nextLvl)
            {
                var buttonNextLevel = new TextButton(
                    _content,
                    buttonCenters[1],
                    buttonTexts[1],
                    "square",
                    font,
                    _colorPallete[0],
                    _colorPallete[1],
                    _colorPallete[3]);
                buttonNextLevel.ButtonEventHandler += NextLevel;
                _buttons.Add(buttonNextLevel);
            }

            var buttonMenu = new TextButton(
                _content,
                buttonCenters[nextLvl ? 2 : 1],
                buttonTexts[nextLvl ? 2 : 1],
                "square",
                font,
                _colorPallete[0],
                _colorPallete[1],
                _colorPallete[3]);
            buttonMenu.ButtonEventHandler += ExitToMenu;
            _buttons.Add(buttonMenu);

            _mouseStateLast = Mouse.GetState();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            foreach (var button in _buttons) button.Update(_mouseStateLast, Mouse.GetState());

            _mouseStateLast = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            base.Draw(gameTime);

            var texture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { _colorPallete[3] });

            spriteBatch.Draw(
                texture,
                new Rectangle(Point.Zero, Tools.ToPoint(Constants.GAME_FULL)),
                Color.White);

            foreach (var button in _buttons) button.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void ReplayLevel(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new TileMapLevelScreen(this._currLvl.CurrLevel), null);
            ScreenManager.RemoveScreen(_currLvl);
        }

        private void NextLevel(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new TileMapLevelScreen(++this._currLvl.CurrLevel), null);
            ScreenManager.RemoveScreen(_currLvl);
        }

        private void ExitToMenu(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.RemoveScreen(_currLvl);

            ScreenManager.AddScreen(new IntroScreen(), null);
        }
    }
}
