using Plugin.Media;
using Plugin.Media.Abstractions;

namespace AssinaturaDigital.Plugins
{
    public static class Media
    {
        private static IMedia _media;

        public static IMedia Instance
        {
            get => _media ?? (_media = CrossMedia.Current);
            set => _media = value;
        }
    }
}
