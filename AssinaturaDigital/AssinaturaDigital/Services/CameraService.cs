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
        private readonly ICameraOverlayProvider _cameraOverlayProvider;

        public CameraService(ICameraOverlayProvider cameraOverlayProvider)
        {
            _media = Media.Instance;
            _options = new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                AllowCropping = false,
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            };
            _cameraOverlayProvider = cameraOverlayProvider;
        }

        public bool CanTakePhoto() => _media.IsCameraAvailable && _media.IsTakePhotoSupported;

        public async Task<MediaFile> TakePhoto(string fileName, CameraDevice camera, string overlayImageName = null)
        {
            if (!CanTakePhoto())
                return null;

            _options.Name = fileName;
            _options.DefaultCamera = camera;

            if (_cameraOverlayProvider != null && !string.IsNullOrEmpty(overlayImageName))
                _options.OverlayViewProvider = _cameraOverlayProvider.GetImage(overlayImageName);

            return await _media.TakePhotoAsync(_options);
        }
    }
}
