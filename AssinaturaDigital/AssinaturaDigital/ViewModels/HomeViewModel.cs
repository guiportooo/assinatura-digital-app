using System;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class HomeViewModel : ViewModelBase, INavigatingAware
    {
        private readonly IPreferences _preferences;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand GoContractListCommand { get; }

        private int IdUser { get; set; }

        public HomeViewModel(IPreferences preferences,
            INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _preferences = preferences;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            LogoutCommand = new DelegateCommand(Logout)
                .ObservesProperty(()=> IsBusy);

            GoContractListCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(ContractListPage));
            });

            Title = "Home";
        }

        async void Logout()
        {
            try
            {
                IsBusy = true;
                _preferences.Clear(AppConstants.IdUser);
                await _navigationService.NavigateAsync(nameof(MainPage));
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(Title, ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            IdUser = _preferences.Get(AppConstants.IdUser, 0);

            if (IdUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");

                await _navigationService.NavigateAsync(nameof(MainPage));
                return;
            }
        }
    }
}
