using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using WrongHole.Screens;
using WrongHole.StateManagement;

namespace WrongHole
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class SpriteExampleGame : Game
    {
        private readonly ScreenManager screenManager;
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Song bgMusic;

        private int score = 0;

        private Random random;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public SpriteExampleGame()
        {
            random = new Random();
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgMusic = Content.Load<Song>("bg_music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgMusic);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (screenManager.GetScreens().Length == 0) screenManager.AddScreen(new RandomLevelScreen(this, random.Next()), null);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void AddInitialScreens()
        {
        }
    }
}
