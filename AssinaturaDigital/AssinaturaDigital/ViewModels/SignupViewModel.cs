using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Authentication;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPreferences _preferences;

        public DelegateCommand SignUpCommand { get; }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        private string _cellPhoneNumber;
        public string CellPhoneNumber
        {
            get => _cellPhoneNumber;
            set => SetProperty(ref _cellPhoneNumber, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private ObservableCollection<Steps> _steps;
        public ObservableCollection<Steps> StepsList
        {
            get => _steps;
            set => SetProperty(ref _steps, value);
        }

        private int _currentStep;
        public int CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }

        private bool FilledUpForm() => !string.IsNullOrEmpty(FullName)
            && !string.IsNullOrEmpty(CPF)
            && !string.IsNullOrEmpty(CellPhoneNumber)
            && !string.IsNullOrEmpty(Email);

        public SignUpViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IAuthenticationService authenticationService,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _authenticationService = authenticationService;

            _preferences = preferences;

            InitializeSteps();

            Title = "Cadastro";

            HasFowardNavigation = false;

            SignUpCommand = new DelegateCommand(SignUp, CanSignUp)
                .ObservesProperty(() => IsBusy)
                .ObservesProperty(() => FullName)
                .ObservesProperty(() => CPF)
                .ObservesProperty(() => CellPhoneNumber)
                .ObservesProperty(() => Email);
        }

        void InitializeSteps()
        {
            CurrentStep = 1;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(false),
                new Steps(false),
                new Steps(false),
                new Steps(false),
            };
        }

        bool CanSignUp() => !IsBusy && FilledUpForm();

        protected async void SignUp()
        {
            try
            {
                IsBusy = true;
                var response = await _authenticationService.SignUp(new SignUpInformation(FullName, CPF, CellPhoneNumber, Email));

                if (!response.Succeeded)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, string.Join("\n", response.ErrorMessages), "OK");
                    return;
                }

                _preferences.Set(AppConstants.IdUser, response.SignUpInformation.Id);

                var parameters = new NavigationParameters
                {
                    { AppConstants.Registered, false }
                };

                await _navigationService.NavigateAsync(nameof(TokenPage), parameters);
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao cadastrar usuário.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
