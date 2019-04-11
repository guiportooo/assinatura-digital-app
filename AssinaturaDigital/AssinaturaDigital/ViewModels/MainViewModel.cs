using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;

namespace AssinaturaDigital.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public DelegateCommand OpenSignUpCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            OpenSignUpCommand = new DelegateCommand(OpenSignUp);
        }

        async void OpenSignUp() => await _navigationService.NavigateAsync(nameof(SignUpPage));
    }
}
