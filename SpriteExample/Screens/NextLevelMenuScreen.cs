﻿using System;
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
    public class NextLevelMenuScreen : GameScreen
    {
        protected List<Button> _buttons;

        protected ContentManager _content;

        protected GraphicsDevice _graphics;

        protected MouseState _mouseStateLast;

        protected Monochrome _monochrome;

        protected TileMapLevelScreen _currLvl; // This screen should be already removed from the screen manager.

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

            _monochrome = Constants.MONOCHROMES[new Random().Next(Constants.MONOCHROMES.Length)];

            var font = _content.Load<SpriteFont>("GNUTypewriter");

            var nextLvl = WrongHoleGame.score[this._currLvl.CurrentLevel] && this._currLvl.CurrentLevel + 1 < WrongHoleGame.score.Length;

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
                _monochrome.Button,
                _monochrome.ButtonSelected,
                _monochrome.ButtonText);
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
                    _monochrome.Button,
                    _monochrome.ButtonSelected,
                    _monochrome.ButtonText);
                buttonNextLevel.ButtonEventHandler += NextLevel;
                _buttons.Add(buttonNextLevel);
            }

            var buttonMenu = new TextButton(
                _content,
                buttonCenters[nextLvl ? 2 : 1],
                buttonTexts[nextLvl ? 2 : 1],
                "square",
                font,
                _monochrome.Button,
                _monochrome.ButtonSelected,
                _monochrome.ButtonText);
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
            texture.SetData(new Color[] { _monochrome.Background });

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

            ScreenManager.AddScreen(new TileMapLevelScreen(this._currLvl.CurrentLevel), null);
        }

        private void NextLevel(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new TileMapLevelScreen(++this._currLvl.CurrentLevel), null);
        }

        private void ExitToMenu(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);

            ScreenManager.AddScreen(new IntroScreen(), null);
        }
    }
}
