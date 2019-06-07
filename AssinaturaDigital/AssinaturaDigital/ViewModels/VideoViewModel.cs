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
    public class VideoViewModel : ViewModelBase, INavigatedAware
    {
        private bool _isSigningContract;
        private ContractData _contract;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly IErrorHandler _errorHandler;

        public VideoViewModel(INavigationService navigationService,
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

            Title = "Video";
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

            TakeVideo().FireAndForget(_errorHandler);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
            => _navigationService.RemoveLastViewWithName(nameof(VideoPage));

        async Task TakeVideo()
        {
            try
            {
                if (!await GrantedCameraPermission())
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Câmera negada.", "OK");
                    await _navigationService.GoBackAsync();
                    return;
                }

                var video = await TakeVideo("Video");
                await NavigateToNextPage(video);
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

        async Task<MediaFile> TakeVideo(string fileName)
        {
            if (!_cameraService.CanTakeVideo())
                throw new InvalidOperationException("Nenhuma câmera detectada.");

            var video = await _cameraService.TakeVideo(fileName, CameraDevice.Front);

            if (video == null)
                throw new NullReferenceException("Não foi possível capturar o vídeo.");

            return video;
        }

        async Task NavigateToNextPage(MediaFile selfie)
        {
            if (_isSigningContract)
            {
                await _navigationService.NavigateAsync(nameof(InfoSigningContractPage),
                    new NavigationParameters
                    {
                        { AppConstants.Contract, _contract },
                        { AppConstants.Video, selfie }
                    });
            }
            else
            {
                await _navigationService.NavigateAsync(nameof(InfoRegisterPage),
                    new NavigationParameters
                    {
                        { AppConstants.Video, selfie }
                    });
            }
        }
    }
}
