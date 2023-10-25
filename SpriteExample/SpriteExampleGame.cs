using System;
using System.IO;
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
        public static int score = 0;
        private readonly ScreenManager screenManager;
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Song bgMusic;
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
            var _tilemap = Content.Load<TileMap>("tilemap");
            bgMusic = Content.Load<Song>("bg_music");
            MediaPlayer.IsRepeating = true;
            //TODO: MediaPlayer.Play(bgMusic);

            score = int.Parse(File.ReadAllText(Constants.SCORE_PATH).Trim());
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (screenManager.GetScreens().Length == 0) screenManager.AddScreen(new TileMapLevelScreen(this, random.Next()), null);
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

        protected override void UnloadContent()
        {
            File.WriteAllText(Constants.SCORE_PATH, score.ToString());

            base.UnloadContent();
        }

        private void AddInitialScreens()
        {
        }
    }
}
