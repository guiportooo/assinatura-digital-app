using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Documents;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class DocumentViewModel : ViewModelBase, INavigatedAware
    {
        private int _idUser;
        private DocumentType _documentType;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly IDocumentsService _documentsService;
        private readonly IPreferences _preferences;
        private readonly IErrorHandler _errorHandler;

        public DocumentViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IPermissionsService permissionsService,
            ICameraService cameraService,
            IDocumentsService documentsService,
            IPreferences preferences,
            IErrorHandler errorHandler) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _permissionsService = permissionsService;
            _cameraService = cameraService;
            _documentsService = documentsService;
            _preferences = preferences;
            _errorHandler = errorHandler;
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                await _navigationService.GoBackAsync();
                return;
            }

            _idUser = _preferences.Get(AppConstants.IdUser, 0);

            if (_idUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");
                GoBack();
                return;
            }

            _documentType = parameters.GetValue<DocumentType>(AppConstants.DocumentType);
            Title = _documentType.ToString();
            TakeDocumentPhotos().FireAndForget(_errorHandler);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
            => _navigationService.RemoveLastViewWithName(nameof(DocumentPage));

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null && parameters[AppConstants.DocumentType] != null;

        async Task TakeDocumentPhotos()
        {
            try
            {
                IsBusy = true;

                if (!await GrantedCameraPermission())
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Câmera negada.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }

                await SavePhoto($"{_documentType}_{DocumentOrientation.Front}", _documentType, DocumentOrientation.Front);
                await SavePhoto($"{_documentType}_{DocumentOrientation.Back}", _documentType, DocumentOrientation.Back);

                await _navigationService.NavigateAsync(nameof(InfoSelfiePage));
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(Title, ex.Message, "OK");
                await _navigationService.GoBackAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task<bool> GrantedCameraPermission()
        {
            var cameraPermission = Permission.Camera;

            if (await _permissionsService.GrantedPermissionTo(cameraPermission))
                return true;

            if (await _permissionsService.ShouldRequestPermissionTo(cameraPermission))
                await _pageDialogService.DisplayAlertAsync(Title, "Permissão necessária para a câmera.", "OK");

            return await _permissionsService.RequestPermissionTo(cameraPermission);
        }

        async Task SavePhoto(string name, DocumentType type, DocumentOrientation orientation)
        {
            var photo = await TakePhoto(name);
            var document = new Document(_idUser, type, orientation, photo);
            await _documentsService.SaveDocument(document);
        }

        async Task<MediaFile> TakePhoto(string fileName)
        {
            if (!_cameraService.CanTakePhoto())
                throw new InvalidOperationException("Nenhuma câmera detectada.");

            var photo = await _cameraService.TakePhoto(fileName, CameraDevice.Rear, fileName);

            if (photo == null)
                throw new NullReferenceException("Não foi possível armazenar a foto.");

            return photo;
        }
    }
}
