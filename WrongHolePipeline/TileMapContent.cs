using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace WrongHolePipeline
{
    [ContentSerializerRuntimeType("WrongHole.TileMap, WrongHole")]
    public class TileMapContent
    {
        public int MapWidth, MapHeight;

        public int TileWidth, TileHeight;

        public Texture2DContent Texture;

        public Rectangle[] Tiles;

        public Vector4[] Borders;

        public int[] TileMapping;

        [ContentSerializerIgnore]
        public String TextureFileName;

        public TileMapContent(string data)
        {
            var sections = data.Split("---").Select(line => line.Trim()).ToList();
            var lines = data.Split('\n');

            this.TextureFileName = sections[0];

            var dimensions = sections[1].Split('\n').Select(line => line.Split(',').Select(x => int.Parse(x)).ToList()).ToList();
            (this.TileWidth, this.TileHeight) = (dimensions[0][0], dimensions[0][1]);

            (this.MapWidth, this.MapHeight) = (dimensions[1][0], dimensions[1][1]);

            this.TileMapping = sections[2].Split(',', '\n').Select(x => int.Parse(x)).ToArray();

            var borders = sections[3].Split('\n').Select(line => line.Split('-', ',').ToArray());
            this.Borders = borders.Select(plane => new Vector4(
                int.Parse(plane[0]) * this.TileWidth,
                int.Parse(plane[1]) * this.TileHeight,
                int.Parse(plane[2]) * this.TileWidth,
                int.Parse(plane[3]) * this.TileHeight)
            ).ToArray();
        }
    }
}
