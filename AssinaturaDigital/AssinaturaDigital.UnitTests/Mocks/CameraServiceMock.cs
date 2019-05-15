using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class CameraServiceMock : ICameraService
    {
        private bool _canTakePhoto;
        private bool _shouldReturnNullPhoto;
        public IList<string> FileNames { get; private set; }
        public IList<CameraDevice> Cameras { get; private set; }
        public IList<MediaFile> Photos { get; set; }

        public CameraServiceMock()
        {
            FileNames = new List<string>();
            Cameras = new List<CameraDevice>();
            Photos = new List<MediaFile>();
        }

        public void ShouldTakePhoto() => _canTakePhoto = true;

        public void ShouldReturnNullPhoto()
        {
            _canTakePhoto = true;
            _shouldReturnNullPhoto = true;
        }

        public bool CanTakePhoto() => _canTakePhoto;

        public Task<MediaFile> TakePhoto(string fileName, CameraDevice camera)
        {
            FileNames.Add(fileName);
            Cameras.Add(camera);
            MediaFile photo = null;

            if (!_shouldReturnNullPhoto)
            {
                photo = new MediaFile(fileName, null);
                Photos.Add(photo);
            }

            return Task.FromResult(photo);
        }
    }
}
