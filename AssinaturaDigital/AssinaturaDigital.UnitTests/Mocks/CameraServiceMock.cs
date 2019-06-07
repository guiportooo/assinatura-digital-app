using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class CameraServiceMock : ICameraService
    {
        private bool _canTakePhoto;
        private bool _canTakeVideo;
        private bool _shouldReturnNullPhoto;
        private bool _shouldReturnNullVideo;
        public IList<string> FileNames { get; private set; }
        public IList<CameraDevice> Cameras { get; private set; }
        public IList<MediaFile> Photos { get; set; }
        public IList<MediaFile> Videos { get; set; }

        public CameraServiceMock()
        {
            FileNames = new List<string>();
            Cameras = new List<CameraDevice>();
            Photos = new List<MediaFile>();
            Videos = new List<MediaFile>();
        }

        public void ShouldTakePhoto() => _canTakePhoto = true;

        public void ShouldReturnNullPhoto()
        {
            _canTakePhoto = true;
            _shouldReturnNullPhoto = true;
        }

        public bool CanTakePhoto() => _canTakePhoto;

        public Task<MediaFile> TakePhoto(string fileName, CameraDevice camera, string overlayImageName = null)
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

        public void ShouldTakeVideo() => _canTakeVideo = true;

        public void ShouldReturnNullVideo()
        {
            _canTakeVideo = true;
            _shouldReturnNullVideo = true;
        }

        public bool CanTakeVideo() => _canTakeVideo;

        public Task<MediaFile> TakeVideo(string fileName, CameraDevice camera)
        {
            FileNames.Add(fileName);
            Cameras.Add(camera);
            MediaFile video = null;

            if (!_shouldReturnNullVideo)
            {
                video = new MediaFile(fileName, null);
                Videos.Add(video);
            }

            return Task.FromResult(video);
        }
    }
}
