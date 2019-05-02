using AssinaturaDigital.Models;
using AssinaturaDigital.Services.SignUp;
using AssinaturaDigital.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;

namespace AssinaturaDigital.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ISignUpService _signUpService;

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

        public SignUpViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ISignUpService signUpService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _signUpService = signUpService;

            InitializeSteps();

            Title = "Cadastro";
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

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;
                var response = await _signUpService.SignUp(new SignUpInformation(FullName, CPF, CellPhoneNumber, Email));

                if (!response.Succeeded)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, string.Join("\n", response.ErrorMessages), "OK");
                    return;
                }

                await _navigationService.NavigateAsync(nameof(TokenPage));
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
