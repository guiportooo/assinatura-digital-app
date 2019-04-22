using AssinaturaDigital.Plugins;
using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.UnitTests.Mocks;
using FluentAssertions;
using NUnit.Framework;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Services
{
    public class CameraServiceTests
    {
        private ICameraService _cameraService;

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void ShouldBeAbleToTakePhotoWhenCameraIsAvailableAndTakePhotoIsSupported(bool isCameraAvailable, bool isTakePhotoSupported, bool canTakePhoto)
        {
            var mediaMock = new MediaMock(isCameraAvailable, isTakePhotoSupported);
            Media.Instance = mediaMock;
            _cameraService = new CameraService();

            _cameraService.CanTakePhoto().Should().Be(canTakePhoto);
        }

        [Test]
        public async Task ShouldTakePhotoWithInformedNameAndCamera()
        {
            const string fileName = "Test";
            var camera = CameraDevice.Rear;

            var expectedOptions = new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                AllowCropping = true,
                RotateImage = false,
                Name = fileName,
                DefaultCamera = camera
            };

            var mediaMock = new MediaMock(true, true);
            Media.Instance = mediaMock;
            _cameraService = new CameraService();

            var photo = await _cameraService.TakePhoto(fileName, camera);

            photo.Should().NotBeNull();
            mediaMock.Options.Should().BeEquivalentTo(expectedOptions);
        }

        [Test]
        public async Task ShouldNotBeAbleToTakePhotoWhenCanNotTakePhoto()
        {
            const string fileName = "Test";
            var camera = CameraDevice.Rear;

            var mediaMock = new MediaMock(false, false);
            Media.Instance = mediaMock;
            _cameraService = new CameraService();

            var photo = await _cameraService.TakePhoto(fileName, camera);

            photo.Should().BeNull();
        }
    }
}
