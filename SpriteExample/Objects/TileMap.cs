using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole
{
    public class TileMap
    {
        public int MapWidth { get; init; }
        public int MapHeight { get; init; }

        public int TileWidth { get; init; }
        public int TileHeight { get; init; }

        public Texture2D Texture { get; init; }

        public Rectangle[] Tiles { get; init; }

        public Vector4[] Borders { get; init; }

        public int[] TileMapping { get; init; }

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
                            x * TileWidth,
                            y * TileHeight,
                            TileWidth,
                            TileHeight
                            ),
                        Tiles[index],
                        color
                        );
                }
            }
        }
    }
}
