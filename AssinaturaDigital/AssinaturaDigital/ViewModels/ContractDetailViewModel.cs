using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
namespace AssinaturaDigital.ViewModels
{
    public class ContractDetailViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IContractService _contractService;

        private ContractData _contractData;
        public ContractData ContractData
        {
            get => _contractData;
            set => SetProperty(ref _contractData, value);
        }

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand GoContractListCommand { get; }

        public ContractDetailViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContractService contractService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _contractService = contractService;

            GoHomeCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(HomePage));
            });

            GoContractListCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(ContractListPage));
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.ContractIdentification))
            {
                var idContract = parameters.GetValue<string>(AppConstants.ContractIdentification);
                ContractData = _contractService.GetContract(idContract);
            }
            else
            {
                await _navigationService.NavigateAsync(nameof(ContractListPage));
            }
        }
    }
}
