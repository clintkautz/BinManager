using Plugin.Media.Abstractions;

namespace camOptions
{
    internal class StoreCameraMediaOptions : Plugin.Media.Abstractions.StoreCameraMediaOptions
    {
        public bool SaveToAlbum { get; set; }
        public string Name { get; set; }
    }
}