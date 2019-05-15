using AssinaturaDigital.Models;
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
using System.Collections.Generic;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class DocumentViewModelTests
    {
        private int _idUser = 1;
        private DocumentViewModel _documentViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private PermissionsServiceMock _permissionsService;
        private CameraServiceMock _cameraService;
        private DocumentsServiceFake _documentsService;
        private Mock<IPreferences> _preferencesMock;
        private ErrorHandlerMock _errorHandler;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _documentsService = new DocumentsServiceFake();
            _permissionsService = new PermissionsServiceMock();
            _cameraService = new CameraServiceMock();
            _preferencesMock = new Mock<IPreferences>();
            _errorHandler = new ErrorHandlerMock();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _documentViewModel = new DocumentViewModel(_navigationService,
                _pageDialogService,
                _permissionsService,
                _cameraService,
                _documentsService,
                _preferencesMock.Object,
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
            const DocumentType documentType = DocumentType.CNH;

            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, documentType }
            };

            _documentViewModel.OnNavigatedTo(parameters);
            _documentViewModel.Title.Should().Be(documentType.ToString());
        }

        [Test]
        public void ShouldNavigateToInfoSelfiePageAfterTakingPhotos()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, DocumentType.RG }
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
                { AppConstants.DocumentType, DocumentType.RG }
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
                { AppConstants.DocumentType, DocumentType.RG }
            };

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Câmera negada.");
            _navigationService.WentBack.Should().BeTrue();
        }

        [TestCase(DocumentType.RG)]
        [TestCase(DocumentType.CNH)]
        public void ShouldSavePhotosAndChangeTitleIfCameraPermissionIsGranted(DocumentType documentType)
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, documentType }
            };

            _permissionsService.GrantedPermissionBeforeRequest();
            _cameraService.ShouldTakePhoto();

            _documentViewModel.OnNavigatedTo(parameters);

            var expectedDocuments = new List<Document>
            {
                new Document(_idUser, documentType, DocumentOrientation.Front, _cameraService.Photos[0]),
                new Document(_idUser, documentType, DocumentOrientation.Back, _cameraService.Photos[1]),
            };

            _documentViewModel.Title.Should().Be(documentType.ToString());
            _cameraService.Cameras[0].Should().Be(CameraDevice.Rear);
            _cameraService.Cameras[1].Should().Be(CameraDevice.Rear);
            _permissionsService.Permission.Should().Be(Permission.Camera);
            _documentsService.Documents.Should().BeEquivalentTo(expectedDocuments);
        }

        [Test]
        public void ShouldDisplayErrorMessageAndGoBackIfPhotoIsNull()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.DocumentType, DocumentType.RG }
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
                { AppConstants.DocumentType, DocumentType.RG }
            };
            _permissionsService.GrantedPermissionBeforeRequest();

            _documentViewModel.OnNavigatedTo(parameters);

            _pageDialogService.Message.Should().Be("Nenhuma câmera detectada.");
            _navigationService.WentBack.Should().BeTrue();
        }
    }
}
