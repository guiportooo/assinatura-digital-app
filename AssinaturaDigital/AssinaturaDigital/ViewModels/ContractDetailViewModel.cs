using AssinaturaDigital.Events;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class ContractDetailViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IContractService _contractService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPreferences _preferences;

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

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand GoContractListCommand { get; }

        public ContractDetailViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContractService contractService,
            IEventAggregator eventAggregator,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _contractService = contractService;
            _eventAggregator = eventAggregator;
            _preferences = preferences;

            GoHomeCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(HomePage));
            });

            GoContractListCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(ContractListPage),
                        new NavigationParameters
                        {
                            { AppConstants.IdUser, _preferences.Get(AppConstants.IdUser, 0) }
                        });
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Unsubscribe(EndOfScroll);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.ContractIdentification))
            {
                var idContract = parameters.GetValue<string>(AppConstants.ContractIdentification);
                Contract = _contractService.GetContract(idContract);
            }
            else
            {
                await _navigationService.NavigateAsync(nameof(ContractListPage),
                        new NavigationParameters
                        {
                            { AppConstants.IdUser, _preferences.Get(AppConstants.IdUser, 0) }
                        });
            }

           _eventAggregator.GetEvent<ScrolledToBottomEvent>().Subscribe(EndOfScroll);
        }

        protected override async void GoFoward()
        {
            base.GoFoward();

            if (AgreeContract)
            {
                var parameters = new NavigationParameters();
                parameters.Add(AppConstants.SigningContract, true);
                parameters.Add(AppConstants.Registered, true);
                parameters.Add(AppConstants.Contract, Contract);
                await _navigationService.NavigateAsync(nameof(InfoSelfiePage), parameters);
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Acetar o contrato", "Necessário aceitar o termo de uso para prosseguir.", "Ok");
            }
        }

        void EndOfScroll() =>
            ReadTerms = true;
    }
}
