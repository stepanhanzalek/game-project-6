using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public float Scale;

        public ((int, int), (string, int))[] Objects;

        public ((int, int), string)[] Texts;

        [ContentSerializerIgnore]
        public String TextureFileName;

        public TileMapContent(string data)
        {
            var sections = Regex.Split(data, @"---.*---").Select(line => line.Trim()).ToList();

            this.TextureFileName = "Textures/" + sections[0];

            var dimensions = sections[1].Split('\n').Select(line => line.Split(',').Select(x => int.Parse(x)).ToList()).ToList();
            (this.TileWidth, this.TileHeight) = (dimensions[0][0], dimensions[0][1]);

            (this.MapWidth, this.MapHeight) = (dimensions[1][0], dimensions[1][1]);

            this.TileMapping = sections[2].Split(',', '\n').Select(x => int.Parse(x)).ToArray();

            var borders = sections[3].Split('\n').Select(line => line.Split('|', ',').ToArray());
            this.Borders = borders.Select(plane => new Vector4(
                int.Parse(plane[0]) * this.TileWidth,
                int.Parse(plane[1]) * this.TileHeight,
                int.Parse(plane[2]) * this.TileWidth,
                int.Parse(plane[3]) * this.TileHeight)
            ).ToArray();

            this.Scale = float.Parse(sections[4]);

            var objects = sections[5].Split('\n').Select(line => line.Split('|').ToArray());
            this.Objects = objects.Select(obj => (
                ((int)(float.Parse(obj[0]) * this.TileWidth), (int)(float.Parse(obj[1]) * this.TileHeight)),
                (obj[2].Trim(), int.Parse(obj[3])))).ToArray();

            if (sections.Count() > 6)
            {
                var texts = sections[6].Split('\n').Select(line => line.Split('|').ToArray());
                this.Texts = texts.Select(obj => (
                    ((int)(float.Parse(obj[0]) * this.TileWidth), (int)(float.Parse(obj[1]) * this.TileHeight)),
                    obj[2].Trim())).ToArray();
            }
            else
            {
                Texts = new ((int, int), string)[] { };
            }
        }
    }
}
