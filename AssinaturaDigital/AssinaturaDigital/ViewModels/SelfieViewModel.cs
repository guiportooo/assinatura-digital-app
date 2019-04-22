using AssinaturaDigital.Extensions;
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
    public class SelfieViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly IDocumentsService _documentsService;
        private readonly IErrorHandler _errorHandler;

        public SelfieViewModel(INavigationService navigationService,
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

            Title = "Selfie";
        }

        public void OnNavigatingTo(INavigationParameters parameters) 
            => TakeSelfiePhoto().FireAndForget(_errorHandler);

        async Task TakeSelfiePhoto()
        {
            try
            {
                if (!await GrantedCameraPermission())
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Câmera negada.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }

                var approvedSelfie = await SavePhoto("Selfie");
                await _navigationService.NavigateAsync(nameof(InfoRegisterPage), 
                    new NavigationParameters 
                    {
                         { AppConstants.ApprovedSelfie, approvedSelfie }
                    },
                    true,
                    true);
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

        async Task<bool> SavePhoto(string name)
        {
            var photo = await TakePhoto(name);
            var approvedSelfie = await _documentsService.SaveSelfie(photo);
            return approvedSelfie;
        }

        async Task<MediaFile> TakePhoto(string fileName)
        {
            if (!_cameraService.CanTakePhoto())
                throw new InvalidOperationException("Nenhuma câmera detectada.");

            var photo = await _cameraService.TakePhoto(fileName, CameraDevice.Front);

            if (photo == null)
                throw new NullReferenceException("Não foi possível armazenar a foto.");

            return photo;
        }
    }
}
