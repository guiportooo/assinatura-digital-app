using AssinaturaDigital.Services.Fakes;
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
    public class SelfieViewModelTests
    {
        private const string pageTitle = "Selfie";
        private SelfieViewModel _selfieViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private PermissionsServiceMock _permissionsService;
        private CameraServiceMock _cameraService;
        private DocumentsServiceFake _documentsService;
        private ErrorHandlerMock _errorHandler;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _documentsService = new DocumentsServiceFake();
            _permissionsService = new PermissionsServiceMock();
            _cameraService = new CameraServiceMock();
            _errorHandler = new ErrorHandlerMock();

            _selfieViewModel = new SelfieViewModel(_navigationService,
                _pageDialogService,
                _permissionsService,
                _cameraService,
                _documentsService,
                _errorHandler);
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitle() => _selfieViewModel.Title.Should().Be(pageTitle);

        [Test]
        public void ShouldNavigateToInfoRegisterPageAfterTakingPhoto()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.ApprovedSelfie, true }
            };

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakePhoto();

            _selfieViewModel.OnNavigatedTo(null);

            _navigationService.Name.Should().Be(nameof(InfoRegisterPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void ShouldDisplayRequestingMessageForCameraPermission()
        {
            _permissionsService.GrantedPermissionAfterRequest();
            _cameraService.ShouldTakePhoto();

            _selfieViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Permissão necessária para a câmera.");
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPermissionToCameraIsNotGranted()
        {
            _selfieViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Câmera negada.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void ShouldSavePhotosIfCameraPermissionIsGranted()
        {
            var expectedPhoto = new MediaFile("Selfie", null);

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakePhoto();

            _selfieViewModel.OnNavigatedTo(null);

            _permissionsService.Permission.Should().Be(Permission.Camera);
            _cameraService.Camera.Should().Be(CameraDevice.Front);
            _documentsService.Photo.Should().BeEquivalentTo(expectedPhoto);
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPhotoIsNull()
        {
            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldReturnNullPhoto();

            _selfieViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Não foi possível armazenar a foto.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfCannotTakePhoto()
        {
            _permissionsService.GrantedPermissionBeforeRequest();

            _selfieViewModel.OnNavigatedTo(null);

            _pageDialogService.Message.Should().Be("Nenhuma câmera detectada.");
            _navigationService.WentBack.Should().BeTrue();
        }
    }
}
