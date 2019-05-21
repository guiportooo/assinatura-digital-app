using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;

namespace AssinaturaDigital.ViewModels
{
    public class InfoSelfieViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        private bool _isSigningContract;

        public DelegateCommand ShowInfoCommand { get; }

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

        private bool _registred;
        public bool Registred
        {
            get => _registred;
            set => SetProperty(ref _registred, value);
        }

        private string _subTitle;
        public string SubTitle
        {
            get => _subTitle;
            set => SetProperty(ref _subTitle, value);
        }

        private ContractData _contract;
        public ContractData Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }

        public InfoSelfieViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            ShowInfoCommand = new DelegateCommand(ShowInfo);

            InitializeSteps();
        }

        void InitializeSteps()
        {
            CurrentStep = 5;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(true),
            };
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.SigningContract))
                _isSigningContract = parameters.GetValue<bool>(AppConstants.SigningContract);

            if (parameters.ContainsKey(AppConstants.Registered))
                Registred = parameters.GetValue<bool>(AppConstants.Registered);

            if (parameters.ContainsKey(AppConstants.Contract))
                Contract = parameters.GetValue<ContractData>(AppConstants.Contract);

            SetCorrectTitleAndSubTitle();
        }

        private void SetCorrectTitleAndSubTitle()
        {
            if (_isSigningContract)
            {
                Title = "Chegou a hora de assinar";
                SubTitle = "Sua foto assinará o contrato automaticamente";
            }
            else
            {
                Title = "Estamos quase terminando";
                SubTitle = "Agora vamos tirar uma selfie";
            }
        }

        async void ShowInfo()
        {
            var info = "Para tornar a assinatura do contrato mais segura e possibilitar maior praticidade no processo, transformaremos sua foto em um algoritmo biométrico para identificação.";
            await _pageDialogService.DisplayAlertAsync(Title, info, "OK");
        }

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                if (_isSigningContract)
                    await _navigationService.NavigateAsync(nameof(SelfieOrientationPage),
                        new NavigationParameters
                        {
                            { AppConstants.Contract, Contract },
                            { AppConstants.SigningContract, _isSigningContract }
                        },
                        useModalNavigation: true,
                        animated: true);
                else
                    await _navigationService.NavigateAsync(nameof(SelfieOrientationPage), useModalNavigation: true, animated: true);
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao avançar para tela de selfie.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
