using AssinaturaDigital.Models;
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
using System.Collections.Generic;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class DocumentViewModelTests
    {
        private DocumentViewModel _documentViewModel;
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

            _documentViewModel = new DocumentViewModel(_navigationService,
                _pageDialogService,
                _permissionsService,
                _cameraService,
                _documentsService,
                _errorHandler);
        }

        [Test]
        public void WhenNavigatingToViewModelShouldGoBackIfDocumentTypeIsNotInformed()
        {
            _documentViewModel.OnNavigatedTo(null);
            _documentViewModel.Title.Should().BeNullOrEmpty();
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void WhenNavigatingToViewModelShouldPopulateTitle()
        {
            const string documentType = "CNH";

            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, documentType }
            };

            _documentViewModel.OnNavigatedTo(parameters);
            _documentViewModel.Title.Should().Be(documentType);
        }

        [Test]
        public void ShouldNavigateToInfoSelfiePageAfterTakingPhotos()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, AppConstants.RG }
            };

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakePhoto();

            _documentViewModel.OnNavigatedTo(parameters);

            _navigationService.Name.Should().Be(nameof(InfoSelfiePage));
        }

        [Test]
        public void ShouldDisplayRequestingMessageForCameraPermission()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, AppConstants.RG }
            };
            _permissionsService.GrantedPermissionAfterRequest();
            _cameraService.ShouldTakePhoto();

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Permissão necessária para a câmera.");
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPermissionToCameraIsNotGranted()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, AppConstants.RG }
            };

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Câmera negada.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [TestCase(AppConstants.RG)]
        [TestCase(AppConstants.CNH)]
        public void ShouldSavePhotosAndChangeTitleIfCameraPermissionIsGranted(string documentType)
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, documentType }
            };
            var expectedPhotos = new Dictionary<PhotoTypes, MediaFile>
            {
                { PhotoTypes.Frontal, new MediaFile($"{documentType}_Frente", null) },
                { PhotoTypes.Back, new MediaFile($"{documentType}_Verso", null) }
            };

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakePhoto();

            _documentViewModel.OnNavigatedTo(parameters);

            _documentViewModel.Title.Should().Be(documentType);
            _cameraService.Camera.Should().Be(CameraDevice.Rear);
            _permissionsService.Permission.Should().Be(Permission.Camera);
            _documentsService.Photos.Should().BeEquivalentTo(expectedPhotos);
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPhotoIsNull()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, AppConstants.RG }
            };
            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldReturnNullPhoto();

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Não foi possível armazenar a foto.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfCannotTakePhoto()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, AppConstants.RG }
            };
            _permissionsService.GrantedPermissionBeforeRequest();

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Nenhuma câmera detectada.");
            _navigationService.WentBack.Should().BeTrue();
        }
    }
}
