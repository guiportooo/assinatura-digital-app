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
        private CameraOverlayProviderMock _cameraOverlayProvider;

        [SetUp]
        public void Setup() => _cameraOverlayProvider = new CameraOverlayProviderMock();

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void ShouldBeAbleToTakePhotoWhenCameraIsAvailableAndTakePhotoIsSupported(bool isCameraAvailable, bool isTakePhotoSupported, bool canTakePhoto)
        {
            var mediaMock = new MediaMock(isCameraAvailable, isTakePhotoSupported);
            Media.Instance = mediaMock;
            _cameraService = new CameraService(_cameraOverlayProvider);

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
                AllowCropping = false,
                RotateImage = true,
                Name = fileName,
                DefaultCamera = camera,
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            };

            var mediaMock = new MediaMock(true, true);
            Media.Instance = mediaMock;
            _cameraService = new CameraService(_cameraOverlayProvider);

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
            _cameraService = new CameraService(_cameraOverlayProvider);

            var photo = await _cameraService.TakePhoto(fileName, camera);

            photo.Should().BeNull();
        }

        [Test]
        public async Task ShouldAddOverlayWhenProviderExistsAndOverlayImageNameIsInformed()
        {
            const string fileName = "Test";
            var camera = CameraDevice.Rear;
            const string overlayImageName = "Overlay Image";
            const string overlayProvider = "Overlay Provider";

            _cameraOverlayProvider.SetProvider(() => overlayProvider);

            var mediaMock = new MediaMock(true, true);
            Media.Instance = mediaMock;
            _cameraService = new CameraService(_cameraOverlayProvider);

            await _cameraService.TakePhoto(fileName, camera, overlayImageName);
            mediaMock.Options.OverlayViewProvider.Invoke().Should().BeEquivalentTo(overlayProvider);
            _cameraOverlayProvider.ImageName.Should().Be(overlayImageName);
            _cameraOverlayProvider.Provider.Invoke().Should().Be(overlayProvider);
        }
    }
}
