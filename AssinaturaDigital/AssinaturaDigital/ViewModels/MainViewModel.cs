using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace AssinaturaDigital.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly INavigationService _navigation;

        public DelegateCommand OpenSignUpCommand { get; }

        public MainViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            OpenSignUpCommand = new DelegateCommand(OpenSignUp);
        }

        async void OpenSignUp() => await _navigation.NavigateAsync(nameof(SignUpPage));
    }
}
