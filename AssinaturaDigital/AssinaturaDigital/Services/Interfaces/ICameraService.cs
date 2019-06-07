using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ICameraService
    {
        bool CanTakePhoto();
        bool CanTakeVideo();
        Task<MediaFile> TakePhoto(string fileName, CameraDevice camera, string overlayImageName = null);
        Task<MediaFile> TakeVideo(string fileName, CameraDevice camera);
    }
}