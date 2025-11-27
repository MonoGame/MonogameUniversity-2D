using System.Collections.Generic;

namespace ContentTypes
{
    /// <summary>
    /// Represents a region within a texture atlas
    /// </summary>
    public class AtlasRegion
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    /// <summary>
    /// Represents texture atlas data loaded from XML
    /// </summary>
    public class TextureAtlasData
    {
        public string Texture { get; set; }
        public List<AtlasRegion> Regions { get; set; }

        public TextureAtlasData()
        {
            Regions = new List<AtlasRegion>();
        }
    }
}
