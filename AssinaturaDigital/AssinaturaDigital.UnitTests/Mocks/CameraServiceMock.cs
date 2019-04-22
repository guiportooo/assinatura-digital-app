using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class CameraServiceMock : ICameraService
    {
        private bool _canTakePhoto;
        private bool _shouldReturnNullPhoto;
        public string FileName { get; private set; }
        public CameraDevice Camera { get; private set; }

        public void ShouldTakePhoto() => _canTakePhoto = true;

        public void ShouldReturnNullPhoto()
        {
            _canTakePhoto = true;
            _shouldReturnNullPhoto = true;
        }

        public bool CanTakePhoto() => _canTakePhoto;

        public Task<MediaFile> TakePhoto(string fileName, CameraDevice camera)
        {
            FileName = fileName;
            Camera = camera;

            if (_shouldReturnNullPhoto)
                return Task.FromResult((MediaFile)null);

            return Task.FromResult(new MediaFile(fileName, null));
        }
    }
}
