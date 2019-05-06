using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand OpenSignUpCommand { get; }
        public DelegateCommand OpenSignInCommand { get; }

        public MainViewModel(INavigationService navigationService,
                    IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            OpenSignUpCommand = new DelegateCommand(OpenSignUp);
            OpenSignInCommand = new DelegateCommand(OpenSignIn);
        }


        private async void OpenSignUp()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(SignUpPage));
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

        private async void OpenSignIn()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(SignInPage));
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
    }
}
