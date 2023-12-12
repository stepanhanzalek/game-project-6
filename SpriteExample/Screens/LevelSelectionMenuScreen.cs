using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WrongHole.Components;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole.Screens
{
    public class LevelSelectionMenuScreen : GameScreen
    {
        protected List<Button> _buttons;

        protected ContentManager _content;

        protected GraphicsDevice _graphics;

        protected MouseState _mouseStateLast;

        protected Monochrome _monochrome;

        public LevelSelectionMenuScreen()
        {
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _graphics = ScreenManager.GraphicsDevice;

            _buttons = new List<Button>();

            _monochrome = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];

            var font = _content.Load<SpriteFont>("GNUTypewriter");

            LevelButton[] levels = new LevelButton[WrongHoleGame.score.Length];
            string[] levelTexts = new string[WrongHoleGame.score.Length];

            for (int i = 0; i < levels.Length; i++) levelTexts[i] = GenerateLevelText(i);

            var levelCenters = Tools.AlignButtonsColumn(levelTexts, font, 3);

            for (int i = 0; i < levels.Length; i++)
            {
                bool playable = true;
                if (i > 0) playable = WrongHoleGame.score[i] || WrongHoleGame.score[i - 1];

                var level = new LevelButton(
                    _content,
                    levelCenters[i],
                    i,
                    levelTexts[i],
                    "button",
                    font,
                    _monochrome.Button,
                    _monochrome.ButtonSelected,
                    _monochrome.ButtonText,
                    !(levelTexts[i] == " ? "));
                level.ButtonEventHandler += PlayLevel;
                _buttons.Add(level);
            }

            var exitButton = new TextButton(
                _content,
                new Point(
                    (int)(font.MeasureString("Back").X / 2 * 1.25f + 15),
                    (int)(Constants.GAME_HEIGHT - font.MeasureString("Back").Y / 2 * 1.25f - 15)),
                "Back",
                "button",
                font,
                _monochrome.Button,
                _monochrome.ButtonSelected,
                _monochrome.ButtonText);
            exitButton.ButtonEventHandler += ExitToMenu;
            _buttons.Add(exitButton);

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

            ScreenManager.GraphicsDevice.Clear(_monochrome.Background);

            spriteBatch.Begin();

            base.Draw(gameTime);

            foreach (var button in _buttons) button.Draw(spriteBatch);

            spriteBatch.End();
        }

        private string GenerateLevelText(int i)
        {
            bool playable = true;
            if (i > 0) playable = WrongHoleGame.score[i] || WrongHoleGame.score[i - 1];
            if (playable && WrongHoleGame.score[i]) return $" {i}.";
            else if (playable && !WrongHoleGame.score[i]) return $" {i} ";
            return " ? ";
        }

        private void PlayLevel(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new TileMapLevelScreen(((LevelButton)sender).Level), null);
        }

        private void ExitToMenu(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new IntroScreen(), null);
        }
    }
}
