using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using WrongHole.Screens;
using WrongHole.StateManagement;

namespace WrongHole
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class WrongHoleGame : Game
    {
        public static bool[] score;
        private readonly ScreenManager screenManager;
        private GraphicsDeviceManager graphics;
        private Song bgMusic;

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
            bgMusic = Content.Load<Song>("bg_music");
            MediaPlayer.IsRepeating = true;
        TODO: MediaPlayer.Play(bgMusic);

            score = File.ReadAllText(Constants.SCORE_PATH).Trim().Split(',').Select(x => bool.Parse(x)).ToArray();
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
            File.WriteAllText(Constants.SCORE_PATH, String.Join(',', score.Select(x => x.ToString())));

            base.UnloadContent();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new IntroScreen(), null);
            screenManager.AddScreen(new SplashScreen(), null);
        }
    }
}
