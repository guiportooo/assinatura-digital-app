using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class SuccessSigningContractViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPreferences _preferences;

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand GoContractListCommand { get; }
        public DelegateCommand GoContractDetailCommand { get; }

        private int IdUser;

        private ContractData _contract;
        public ContractData Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }

        public SuccessSigningContractViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
             IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
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
                            { AppConstants.IdUser, IdUser }
                        });
            });

            GoContractDetailCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(ContractDetailPage),
                        new NavigationParameters
                        {
                            { AppConstants.ContractIdentification, Contract.Identification }
                        });
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            IdUser = _preferences.Get(AppConstants.IdUser, 0);

            if (parameters.ContainsKey(AppConstants.Contract))
                Contract = parameters.GetValue<ContractData>(AppConstants.Contract);
        }
    }
}
