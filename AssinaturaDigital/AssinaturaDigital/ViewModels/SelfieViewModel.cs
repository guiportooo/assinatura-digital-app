using AssinaturaDigital.Extensions;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Services.Selfies;
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
    public class SelfieViewModel : ViewModelBase, INavigatedAware
    {
        private int _idUser;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionsService _permissionsService;
        private readonly ICameraService _cameraService;
        private readonly ISelfiesService _selfiesService;
        private readonly IPreferences _preferences;
        private readonly IErrorHandler _errorHandler;

        public SelfieViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IPermissionsService permissionsService,
            ICameraService cameraService,
            ISelfiesService selfiesService,
            IPreferences preferences,
            IErrorHandler errorHandler) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _permissionsService = permissionsService;
            _cameraService = cameraService;
            _selfiesService = selfiesService;
            _preferences = preferences;
            _errorHandler = errorHandler;

            Title = "Selfie";
        }

        public async void OnNavigatedTo(INavigationParameters parameters) => await Initialize();

        public void OnNavigatedFrom(INavigationParameters parameters)
            => _navigationService.RemoveLastViewWithName(nameof(SelfiePage));

        async Task Initialize()
        {
            _idUser = _preferences.Get(AppConstants.IdUser, 0);

            if (_idUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");
                GoBack();
                return;
            }

            TakeSelfiePhoto().FireAndForget(_errorHandler);
        }

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
                    });
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
            var approvedSelfie = await _selfiesService.SaveSelfie(_idUser, photo);
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
