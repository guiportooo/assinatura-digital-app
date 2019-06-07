using AssinaturaDigital.Plugins;
using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public class CameraService : ICameraService
    {
        private readonly IMedia _media;
        private readonly StoreCameraMediaOptions _photoOptions;
        private readonly StoreVideoOptions _videoOptions;
        private readonly ICameraOverlayProvider _cameraOverlayProvider;

        public CameraService(ICameraOverlayProvider cameraOverlayProvider)
        {
            _media = Media.Instance;
            _photoOptions = new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                AllowCropping = false,
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            };
            _videoOptions = new StoreVideoOptions
            {
                SaveToAlbum = true,
                DesiredLength = TimeSpan.FromSeconds(3),
                Quality = VideoQuality.Medium
            };
            _cameraOverlayProvider = cameraOverlayProvider;
        }

        public bool CanTakePhoto() => _media.IsCameraAvailable && _media.IsTakePhotoSupported;

        public bool CanTakeVideo() => _media.IsCameraAvailable && _media.IsTakeVideoSupported;

        public async Task<MediaFile> TakePhoto(string fileName, CameraDevice camera, string overlayImageName = null)
        {
            if (!CanTakePhoto())
                return null;

            _photoOptions.Name = fileName;
            _photoOptions.DefaultCamera = camera;

            if (_cameraOverlayProvider.CanSetOverlay() && !string.IsNullOrEmpty(overlayImageName))
                _photoOptions.OverlayViewProvider = _cameraOverlayProvider.GetImage(overlayImageName);

            return await _media.TakePhotoAsync(_photoOptions);
        }

        public async Task<MediaFile> TakeVideo(string fileName, CameraDevice camera)
        {
            if (!CanTakePhoto())
                return null;

            _videoOptions.Name = fileName;
            _videoOptions.DefaultCamera = camera;
            return await _media.TakeVideoAsync(_videoOptions);
        }
    }
}
