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
    public class SelfieViewModel : ViewModelBase, INavigatedAware
    {
        private bool _isSigningContract;
        private ContractData _contract;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly IErrorHandler _errorHandler;

        public SelfieViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IPermissionsService permissionsService,
            ICameraService cameraService,
            IErrorHandler errorHandler) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _permissionsService = permissionsService;
            _cameraService = cameraService;
            _errorHandler = errorHandler;

            Title = "Selfie";
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(AppConstants.SigningContract))
                    _isSigningContract = parameters.GetValue<bool>(AppConstants.SigningContract);

                if (parameters.ContainsKey(AppConstants.Contract))
                    _contract = parameters.GetValue<ContractData>(AppConstants.Contract);
            }

            TakeSelfie().FireAndForget(_errorHandler);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
            => _navigationService.RemoveLastViewWithName(nameof(SelfiePage));

        async Task TakeSelfie()
        {
            try
            {
                if (!await GrantedCameraPermission())
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Câmera negada.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }

                var selfie = await TakePhoto("Selfie");
                await NavigateToNextPage(selfie);
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

        async Task<MediaFile> TakePhoto(string fileName)
        {
            if (!_cameraService.CanTakePhoto())
                throw new InvalidOperationException("Nenhuma câmera detectada.");

            var photo = await _cameraService.TakePhoto(fileName, CameraDevice.Front);

            if (photo == null)
                throw new NullReferenceException("Não foi possível armazenar a foto.");

            return photo;
        }

        async Task NavigateToNextPage(MediaFile selfie)
        {
            if (_isSigningContract)
            {
                await _navigationService.NavigateAsync(nameof(InfoSigningContractPage),
                    new NavigationParameters
                    {
                        { AppConstants.Contract, _contract },
                        { AppConstants.Selfie, selfie }
                    });
            }
            else
            {
                await _navigationService.NavigateAsync(nameof(InfoRegisterPage),
                    new NavigationParameters
                    {
                        { AppConstants.Selfie, selfie }
                    });
            }
        }
    }
}
