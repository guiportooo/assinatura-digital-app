using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SelfieViewModelTests
    {
        private readonly int _idUser = 1;
        private const string pageTitle = "Selfie";
        private SelfieViewModel _selfieViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private PermissionsServiceMock _permissionsService;
        private CameraServiceMock _cameraService;
        private SelfiesServiceFake _selfiesService;
        private ErrorHandlerMock _errorHandler;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _selfiesService = new SelfiesServiceFake();
            _permissionsService = new PermissionsServiceMock();
            _cameraService = new CameraServiceMock();
            _errorHandler = new ErrorHandlerMock();
            _preferencesMock = new Mock<IPreferences>();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _selfieViewModel = new SelfieViewModel(_navigationService,
                _pageDialogService,
                _permissionsService,
                _cameraService,
                _selfiesService,
                _preferencesMock.Object,
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
            _selfiesService.Photo.Should().BeEquivalentTo(expectedPhoto);
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
