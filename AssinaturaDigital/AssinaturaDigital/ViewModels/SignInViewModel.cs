using AssinaturaDigital.Views;
using Prism.Navigation;
using Prism.Services;

namespace AssinaturaDigital.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        public SignInViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            Title = "Confirmação ";
        }

        protected override bool CanGoFoward() => !IsBusy;

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                var parameters = new NavigationParameters();
                parameters.Add("ShowStep", false);
                parameters.Add("CPF", CPF);

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
