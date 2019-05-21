using AssinaturaDigital.Events;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Contracts;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class ContractDetailViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IContractsService _contractsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPreferences _preferences;

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand GoContractListCommand { get; }

        private ContractData _contract;
        public ContractData Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }

        private bool _agreeContract;
        public bool AgreeContract
        {
            get => _agreeContract;
            set => SetProperty(ref _agreeContract, value);
        }

        private bool _readTerms;
        public bool ReadTerms
        {
            get => _readTerms;
            set => SetProperty(ref _readTerms, value);
        }

        public ContractDetailViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContractsService contractsService,
            IEventAggregator eventAggregator,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _contractsService = contractsService;
            _eventAggregator = eventAggregator;
            _preferences = preferences;

            Title = "Detalhes do contrato";

            GoHomeCommand = new DelegateCommand(GoHome).ObservesProperty(() => IsBusy);
            GoContractListCommand = new DelegateCommand(GoToContractsList).ObservesProperty(() => IsBusy);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
            => _eventAggregator.GetEvent<ScrolledToBottomEvent>().Unsubscribe(EndOfScroll);

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                GoBack();
                return;
            }

            Contract = parameters.GetValue<ContractData>(AppConstants.Contract);
            HasFowardNavigation = !Contract.IsSigned;

            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Subscribe(EndOfScroll);
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null
            && parameters[AppConstants.Contract] != null;

        void EndOfScroll() =>
            ReadTerms = true;

        private async void GoToContractsList()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(ContractListPage));
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao navegar para listagem de contratos.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                if (AgreeContract)
                {
                    var parameters = new NavigationParameters
                    {
                        { AppConstants.SigningContract, true },
                        { AppConstants.Registered, true },
                        { AppConstants.Contract, Contract }
                    };
                    await _navigationService.NavigateAsync(nameof(InfoSelfiePage), parameters);
                }
                else
                    await _pageDialogService.DisplayAlertAsync(Title, "Necessário aceitar o termo de uso para prosseguir.", "OK");
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        protected override async void GoBack()
        {
            try
            {
                IsBusy = true;

                await _navigationService.NavigateAsync(nameof(ContractListPage));
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
