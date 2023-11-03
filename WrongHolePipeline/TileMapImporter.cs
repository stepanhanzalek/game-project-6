using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace WrongHolePipeline
{
    using TImport = TileMapContent;

    [ContentImporter(".tmap", DisplayName = "TileMapImporter", DefaultProcessor = "TileMapProcessor")]
    public class TileMapImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return new TImport(File.ReadAllText(filename));
        }
    }
}
