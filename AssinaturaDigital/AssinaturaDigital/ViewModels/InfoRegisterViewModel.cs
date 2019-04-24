using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class InfoRegisterViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand NavigateToHomeCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand GoBackToSelfieCommand { get; }

        private bool _approved;
        public bool Approved
        {
            get => _approved;
            set => SetProperty(ref _approved, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public InfoRegisterViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            HasFowardNavigation = false;

            NavigateToHomeCommand = new DelegateCommand(NavigateToHome, CanNavigate)
                .ObservesProperty(() => IsBusy);
            LogoutCommand = new DelegateCommand(Logout, CanNavigate)
                .ObservesProperty(() => IsBusy);
            GoBackToSelfieCommand = new DelegateCommand(GoBack, CanNavigate)
                .ObservesProperty(() => IsBusy);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                GoBack();
                return;
            }

            Approved = (bool)parameters[AppConstants.ApprovedSelfie];
            SetMessage();
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null && parameters[AppConstants.ApprovedSelfie] != null;

        void SetMessage()
        {
            if (Approved)
            {
                Title = "Quase pronto para assinar!";
                Message = "Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.";
            }
            else
            {
                Title = "Imagem inválida!";
                Message = "Você será redirecionado para uma nova selfie.";
            }
        }

        bool CanNavigate() => !IsBusy;

        async void NavigateToHome()
        {
            try
            {
                IsBusy = true;
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

        async void Logout()
        {
            try
            {
                IsBusy = true;
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
    }
}
