using AssinaturaDigital.Models;
using AssinaturaDigital.Services;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly IPageDialogService _pageDialogService;
        private readonly ISignUpService _signUpService;

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

        private string _cellphoneNumber;
        public string CellphoneNumber
        {
            get => _cellphoneNumber;
            set => SetProperty(ref _cellphoneNumber, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public SignUpViewModel(INavigationService navigation, IPageDialogService pageDialogService, ISignUpService signUpService)
        {
            _navigation = navigation;
            _pageDialogService = pageDialogService;
            _signUpService = signUpService;

            Title = "Cadastro";

            SignUpCommand = new DelegateCommand(SignUp, CanSignUp)
                .ObservesProperty(() => IsBusy);
        }

        bool CanSignUp() => !IsBusy;

        async void SignUp()
        {
            try
            {
                IsBusy = true;
                await _signUpService.SignUp(new SignUpInformation(FullName, CPF, CellphoneNumber, Email));
                await _navigation.NavigateAsync(nameof(TokenPage));
            }
            catch (InvalidOperationException ex)
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
