using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using WrongHole.Screens;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class WrongHoleGame : Game
    {
        public static bool[] score;
        public static int hiScore;
        private static Song bgMusic;
        private readonly ScreenManager screenManager;
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public WrongHoleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            graphics.ApplyChanges();

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            this.screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            bgMusic = Content.Load<Song>(Constants.SOUND_PATH + "bg_music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgMusic);

            var progressFile = File.ReadAllText(Constants.SCORE_PATH).Trim().Split(',');

            hiScore = int.Parse(progressFile[0]);
            score = progressFile.TakeLast(progressFile.Length - 1).Select(x => bool.Parse(x)).ToArray();
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (screenManager.GetScreens().Length == 0) Exit();
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            var progressFile = new List<string>();
            progressFile.Add(hiScore.ToString());
            progressFile.AddRange(score.Select(x => x.ToString()));

            File.WriteAllText(Constants.SCORE_PATH, String.Join(',', progressFile));

            base.UnloadContent();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new IntroScreen(), null);
            screenManager.AddScreen(new SplashScreen(), null);
        }
    }
}
