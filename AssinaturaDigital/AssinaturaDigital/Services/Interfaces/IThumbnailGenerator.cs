using System.IO;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface IThumbnailGenerator
    {
        Stream GenerateThumbImage(string url, long usecond);
    }
}
