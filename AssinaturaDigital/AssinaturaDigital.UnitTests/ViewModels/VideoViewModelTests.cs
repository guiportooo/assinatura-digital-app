using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class VideoViewModelTests
    {
        private const string pageTitle = "Video";
        private VideoViewModel _videoViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private PermissionsServiceMock _permissionsService;
        private CameraServiceMock _cameraService;
        private ErrorHandlerMock _errorHandler;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _permissionsService = new PermissionsServiceMock();
            _cameraService = new CameraServiceMock();
            _errorHandler = new ErrorHandlerMock();

            _videoViewModel = new VideoViewModel(_navigationService,
                _pageDialogService,
                _permissionsService,
                _cameraService,
                _errorHandler);
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitle() => _videoViewModel.Title.Should().Be(pageTitle);

        [Test]
        public void ShouldNavigateToInfoRegisterPageAfterTakingVideo()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.Video, new MediaFile("Video", null) }
            };

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakeVideo();

            _videoViewModel.OnNavigatedTo(null);

            _navigationService.Name.Should().Be(nameof(InfoRegisterPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void ShouldDisplayRequestingMessageForCameraPermission()
        {
            _permissionsService.GrantedPermissionAfterRequest();
            _cameraService.ShouldTakeVideo();

            _videoViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Permissão necessária para a câmera.");
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPermissionToCameraIsNotGranted()
        {
            _videoViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Câmera negada.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void ShouldTakeVideoIfCameraPermissionIsGranted()
        {
            var expectedVideo = new MediaFile("Video", null);

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakeVideo();

            _videoViewModel.OnNavigatedTo(null);

            _permissionsService.Permission.Should().Be(Permission.Camera);
            _cameraService.Cameras[0].Should().Be(CameraDevice.Front);
            _cameraService.Videos[0].Should().BeEquivalentTo(expectedVideo);
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfVideoIsNull()
        {
            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldReturnNullVideo();

            _videoViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Não foi possível capturar o vídeo.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfCannotTakeVideo()
        {
            _permissionsService.GrantedPermissionBeforeRequest();

            _videoViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Nenhuma câmera detectada.");
            _navigationService.WentBack.Should().BeTrue();
        }
    }
}
