using AssinaturaDigital.Models;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class ContractListViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public ObservableCollection<ContractData> Contracts { get; set; }

        public ContractListViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            Title = "Contratos";

            Contracts = new ObservableCollection<ContractData>()
            {
                new ContractData("IDENTIFICAÇÃO 01", "000.000.000-00", false),
                new ContractData("IDENTIFICAÇÃO 02", "000.000.000-00", true),
                new ContractData("IDENTIFICAÇÃO 03", "000.000.000-00", false),
                new ContractData("IDENTIFICAÇÃO 04", "000.000.000-00", false)
            };
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
