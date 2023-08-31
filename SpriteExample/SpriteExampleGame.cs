using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpriteExample
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class SpriteExampleGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SlimeGhostSprite slimeGhost;

        private Texture2D atlas;

        private SpriteFont bangers;

        private BatSprite[] bats;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public SpriteExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            slimeGhost = new SlimeGhostSprite();

            bats = new BatSprite[]
            {
                new BatSprite(){Position = new Vector2(100, 100), Direction = Direction.Down},
                new BatSprite(){Position = new Vector2(40, 10), Direction = Direction.Up},
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            slimeGhost.LoadContent(Content);
            atlas = Content.Load<Texture2D>("colored_packed");
            foreach (BatSprite bat in bats) bat.LoadContent(Content);
            bangers = Content.Load<SpriteFont>("bangers");
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            slimeGhost.Update(gameTime);
            foreach (BatSprite bat in bats) bat.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.DrawString(bangers, $"{gameTime.TotalGameTime:c}", new Vector2(100, 80), Color.White);
            slimeGhost.Draw(gameTime, spriteBatch);
            foreach (BatSprite bat in bats) bat.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
