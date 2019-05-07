using AssinaturaDigital.Services.SignUp;
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
        private readonly ISignUpService _signUpService;
        private readonly IPreferences _preferences;

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        public SignInViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ISignUpService signUpService,
            IPreferences preferences)
            : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _signUpService = signUpService;
            _preferences = preferences;

            Title = "Confirmação ";
        }

        protected override bool CanGoFoward() => !IsBusy;

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                var response = await _signUpService.GetByCPF(CPF);
                if (!response.Succeeded)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido", "OK");
                    return;
                }

                _preferences.Set(AppConstants.IdUser, response.SignUpInformation.Id);

                var parameters = new NavigationParameters
                {
                    { AppConstants.ShowSteps, false }
                };

                await _navigationService.NavigateAsync(nameof(TokenPage), parameters);
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao encontrar usuário.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
