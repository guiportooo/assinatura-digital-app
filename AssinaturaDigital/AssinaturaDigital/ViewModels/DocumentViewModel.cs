using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.ViewModels
{
    public class DocumentViewModel : ViewModelBase, INavigatingAware
    {
        private string _documentType;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly IDocumentsService _documentsService;
        private readonly IErrorHandler _errorHandler;

        public DocumentViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IPermissionsService permissionsService,
            ICameraService cameraService,
            IDocumentsService documentsService,
            IErrorHandler errorHandler)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _permissionsService = permissionsService;
            _cameraService = cameraService;
            _documentsService = documentsService;
            _errorHandler = errorHandler;
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                await _navigationService.GoBackAsync();
                return;
            }

            _documentType = parameters[AppConstants.DocumentType].ToString();
            Title = $"{_documentType}_Frente";
            TakeDocumentPhotos().FireAndForget(_errorHandler);
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null && parameters[AppConstants.DocumentType] != null;

        async Task TakeDocumentPhotos()
        {
            try
            {
                if (!await GrantedCameraPermission())
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Câmera negada.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }

                await SavePhoto($"{_documentType}_Frente", PhotoTypes.Frontal);

                Title = $"{_documentType}_Verso";

                await SavePhoto($"{_documentType}_Verso", PhotoTypes.Back);

                await _navigationService.NavigateAsync(nameof(InfoSelfiePage));
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(Title, ex.Message, "OK");
                await _navigationService.GoBackAsync();
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

        async Task SavePhoto(string name, PhotoTypes type)
        {
            var photo = await TakePhoto(name);

            switch(_documentType)
            {
                case AppConstants.RG:
                    await _documentsService.SaveRG(photo, type);
                    break;
                case AppConstants.CNH:
                    await _documentsService.SaveCNH(photo, type);
                    break;
            }
        }

        async Task<MediaFile> TakePhoto(string fileName)
        {
            if (!_cameraService.CanTakePhoto())
                throw new InvalidOperationException("Nenhuma câmera detectada.");

            var photo = await _cameraService.TakePhoto(fileName, CameraDevice.Rear);

            if (photo == null)
                throw new NullReferenceException("Não foi possível armazenar a foto.");

            return photo;
        }
    }
}
