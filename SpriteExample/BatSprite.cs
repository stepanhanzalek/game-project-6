using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteExample
{
    public enum Direction
    {
        Down,
        Right,
        Up,
        Left,
    }

    public class BatSprite
    {
        public Direction Direction;
        public Vector2 Position;

        private Texture2D texture;

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("32x32-bat-sprite");
        }

        public void Update(GameTime gameTime)
        {
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (directionTimer > 2)
            {
                switch (Direction)
                {
                    case Direction.Up:
                        Direction = Direction.Down;
                        break;

                    case Direction.Down:
                        Direction = Direction.Right;
                        break;

                    case Direction.Right:
                        Direction = Direction.Left;
                        break;

                    default:
                        Direction = Direction.Up;
                        break;
                }

                directionTimer = 0;
            }

            switch (Direction)
            {
                case Direction.Up:
                    Position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case Direction.Down:
                    Position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case Direction.Left:
                    Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case Direction.Right:
                    Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.3)
            {
                ++animationFrame;
                animationFrame %= 3;
                animationTimer = 0;
            }
            var source = new Rectangle(animationFrame * 32, (int)Direction * 32, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}
