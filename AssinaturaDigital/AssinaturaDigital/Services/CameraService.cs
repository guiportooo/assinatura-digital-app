using AssinaturaDigital.Plugins;
using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public class CameraService : ICameraService
    {
        private readonly IMedia _media;
        private readonly StoreCameraMediaOptions _options;

        public CameraService()
        {
            _media = Media.Instance;
            _options = new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                AllowCropping = true,
                RotateImage = false,
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 80
            };
        }

        public bool CanTakePhoto() => _media.IsCameraAvailable && _media.IsTakePhotoSupported;

        public async Task<MediaFile> TakePhoto(string fileName, CameraDevice camera)
        {
            if (!CanTakePhoto())
                return null;

            _options.Name = fileName;
            _options.DefaultCamera = camera;
            return await _media.TakePhotoAsync(_options);
        }
    }
}
