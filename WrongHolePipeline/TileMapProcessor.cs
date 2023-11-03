using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace WrongHolePipeline
{
    using TInput = TileMapContent;

    using TOutput = TileMapContent;

    [ContentProcessor(DisplayName = "TileMapProcessor")]
    public class TileMapProcessor : ContentProcessor<TInput, TOutput>
    {
        public float Scale { get; set; } = 1f;

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            input.Texture = context.BuildAndLoadAsset<TextureContent, Texture2DContent>(new ExternalReference<TextureContent>(input.TextureFileName), "TextureProcessor");

            int tilesetColumns = input.TileWidth;
            int tilesetRows = input.TileHeight;

            input.Tiles = new Rectangle[tilesetColumns * tilesetRows];
            context.Logger.LogMessage($"{input.Tiles.Length} Total tiles");
            for (int y = 0; y < tilesetRows; y++)
            {
                for (int x = 0; x < tilesetColumns; x++)
                {
                    // The Tiles array provides the source rectangle for a tile
                    // within the tileset texture
                    input.Tiles[y * tilesetColumns + x] = new Rectangle(
                        x * input.TileWidth,
                        y * input.TileHeight,
                        input.TileWidth,
                        input.TileHeight
                        );
                }
            }

            input.TileWidth = (int)(input.TileWidth * Scale);
            input.TileHeight = (int)(input.TileHeight * Scale);

            return input;
        }
    }
}
