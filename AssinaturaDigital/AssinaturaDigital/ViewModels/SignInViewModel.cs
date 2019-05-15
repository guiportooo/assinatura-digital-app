using AssinaturaDigital.Services.Authentication;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPreferences _preferences;

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        public SignInViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IAuthenticationService authenticationService,
            IPreferences preferences)
            : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _authenticationService = authenticationService;
            _preferences = preferences;

            Title = "Confirmação";
        }

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                var response = await _authenticationService.GetByCPF(CPF);

                if (!response.Succeeded)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "CPF não cadastrado. Por favor, acesse a página inicial e clique em 1º acesso.", "OK");
                    return;
                }

                _preferences.Set(AppConstants.IdUser, response.SignUpInformation.Id);

                var parameters = new NavigationParameters
                {
                    { AppConstants.Registered, true }
                };

                await _navigationService.NavigateAsync(nameof(TokenPage), parameters);
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao validar usuário.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
