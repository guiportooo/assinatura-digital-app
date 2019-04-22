using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ICameraService
    {
        bool CanTakePhoto();
        Task<MediaFile> TakePhoto(string fileName, CameraDevice camera);
    }
}