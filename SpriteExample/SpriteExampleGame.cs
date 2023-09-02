using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteExample.Content;

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
        private SpriteFont bangersSmall;

        private Ball ball;

        private Vector2 textSize;

        private Hole[] holes;

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

            ball = new Ball();

            holes = new Hole[] {
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
                new Hole(),
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var hole in holes) hole.LoadContent(GraphicsDevice, Content, "hole1");
            holes[0].LoadContent(GraphicsDevice, Content, "hole2", new Vector2(490, 240));
            holes[0].dummy = false;
            // TODO: use this.Content to load your game content here
            slimeGhost.LoadContent(Content);
            atlas = Content.Load<Texture2D>("colored_packed");
            ball.LoadContent(GraphicsDevice, Content, "ball");
            bangers = Content.Load<SpriteFont>("bangers");
            bangersSmall = Content.Load<SpriteFont>("bangers1");
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            foreach (var hole in holes)
            {
                if (ball.Bounds.CollidesWith(hole.Bounds) && !hole.dummy) Exit();
            }
            ball.Update(gameTime, GraphicsDevice);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            textSize = bangers.MeasureString("WRONG H  LEs");

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var hole in holes) hole.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.DrawString(
                bangers, "WRONG H  LEs",
                new Vector2(
                    GraphicsDevice.Viewport.Width / 2,
                    GraphicsDevice.Viewport.Height / 2)
                - textSize / 2,
                Color.White);
            spriteBatch.DrawString(
                bangersSmall, "Left click and drag to set the direction and speed of the ball. Find the right hole to exit.",
                new Vector2(0,
                    GraphicsDevice.Viewport.Height - 25),
                Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
