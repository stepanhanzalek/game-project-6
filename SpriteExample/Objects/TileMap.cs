using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using Vector2A = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole
{
    public class TileMap
    {
        public ((int X, int Y) Position, (string Type, int Lifespan) Data)[] Objects;
        public ((int X, int Y) Position, string Text)[] Texts;
        public int MapWidth { get; init; }
        public int MapHeight { get; init; }

        public int TileWidth { get; init; }
        public int TileHeight { get; init; }

        public Texture2D Texture { get; init; }

        public Rectangle[] Tiles { get; init; }

        public Vector4[] Borders { get; init; }

        public int[] TileMapping { get; init; }

        public float Scale { get; init; }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = TileMapping[y * MapWidth + x];

                    // Index of -1 (shifted from 0) should not be drawn
                    if (index == -1) continue;

                    // Draw the current tile
                    spriteBatch.Draw(
                        Texture,
                        new Rectangle(
                            (int)(Constants.GAME_WIDTH / 2 + (-(float)MapWidth / 2 + x) * TileWidth * Scale),
                            (int)(Constants.GAME_HEIGHT / 2 + (-(float)MapHeight / 2 + y) * TileHeight * Scale),
                            (int)(Scale * TileWidth),
                            (int)(Scale * TileHeight)
                            ),
                        Tiles[index],
                        color
                        );
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color, SpriteFont font)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = TileMapping[y * MapWidth + x];

                    // Index of -1 (shifted from 0) should not be drawn
                    if (index == -1) continue;

                    // Draw the current tile
                    spriteBatch.Draw(
                        Texture,
                        new Rectangle(
                            (int)(Constants.GAME_WIDTH / 2 + (-(float)MapWidth / 2 + x) * TileWidth * Scale),
                            (int)(Constants.GAME_HEIGHT / 2 + (-(float)MapHeight / 2 + y) * TileHeight * Scale),
                            (int)(Scale * TileWidth),
                            (int)(Scale * TileHeight)
                            ),
                        Tiles[index],
                        color
                        );
                }
            }

            if (Texts.Length != 0)
            {
                foreach (var Text in Texts)
                {
                    spriteBatch.DrawString(
                        font,
                        Text.Text,
                        new Vector2((int)(Constants.GAME_WIDTH / 2 - (float)MapWidth / 2 * TileWidth * Scale),
                                (int)(Constants.GAME_HEIGHT / 2 - (float)MapHeight / 2 * TileHeight * Scale))
                            - font.MeasureString(Text.Text) / 2
                            + new Vector2(Text.Position.X, Text.Position.Y) * Scale,
                        color);
                }
            }
        }

        public void AddBorders(World world)
        {
            foreach (var border in this.Borders)
            {
                Vector2A center = new Vector2A(
                    Constants.GAME_WIDTH / 2 - (float)MapWidth / 2 * TileWidth * Scale,
                    Constants.GAME_HEIGHT / 2 - (float)MapHeight / 2 * TileHeight * Scale);
                var X = new Vector2A(border.X, border.Y);
                var edge = world.CreateEdge(
                    new Vector2A(border.X, border.Y) * Scale + center,
                    new Vector2A(border.Z, border.W) * Scale + center);
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1);
            }
        }

        public Vector2A GetPos((int X, int Y) pos)
        {
            return new Vector2A(
                    Constants.GAME_WIDTH / 2 - (float)MapWidth / 2 * TileWidth * Scale + pos.X * Scale,
                    Constants.GAME_HEIGHT / 2 - (float)MapHeight / 2 * TileHeight * Scale + pos.Y * Scale);
        }
    }
}
