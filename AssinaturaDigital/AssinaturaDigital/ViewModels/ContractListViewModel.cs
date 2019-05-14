using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class ContractListViewModel : ViewModelBase, INavigatedAware
    {
        private int _idUser;
        private List<ContractData> _contractList;
        private readonly INavigationService _navigationService;
        private readonly IContractService _contractService;
        private readonly IPreferences _preferences;

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand<string> SearchContractCommand { get; }
        public DelegateCommand<string> OpenContractDetailsCommand { get; }

        private ObservableCollection<ContractData> _contracts;
        public ObservableCollection<ContractData> Contracts
        {
            get => _contracts;
            set => SetProperty(ref _contracts, value);
        }

        public ContractListViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContractService contractService,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _contractService = contractService;
            _preferences = preferences;

            Title = "Contratos";

            Contracts = new ObservableCollection<ContractData>();
            _contractList = new List<ContractData>();

            GoHomeCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync(nameof(HomePage));
            });
            SearchContractCommand = new DelegateCommand<string>(SearchContract);
            OpenContractDetailsCommand = new DelegateCommand<string>(OpenContractDetails).ObservesProperty(() => IsBusy);
        }

        private void OpenContractDetails(string identifier)
        {
            try
            {
                IsBusy = true;
                var parameters = new NavigationParameters
                {
                    { AppConstants.ContractIdentification, identifier }
                };
                _navigationService.NavigateAsync(nameof(ContractDetailPage), parameters);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SearchContract(string text)
        {
            var contracts = Contracts.Where(x => x.Identification.Contains(text)).ToList();
            if (contracts != null && contracts.Count > 0)
            {
                Contracts = new ObservableCollection<ContractData>();
                contracts.ForEach(Contracts.Add);
            }
        }

        public void ReloadContractList(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Contracts = new ObservableCollection<ContractData>();
                _contractList.ForEach(Contracts.Add);
            }
        }

        public void FiltreContracts(bool showSigned)
        {
            Contracts = new ObservableCollection<ContractData>();
            if (showSigned)
                _contractList?.ForEach(Contracts.Add);
            else
                ShowOnlyUnsignedContracts();
        }

        private void ShowOnlyUnsignedContracts()
        {
            var filtred = _contractList?.Where(x => x.IsSigned == false)?.ToList();
            if (filtred != null)
                filtred.ForEach(Contracts.Add);
        }

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                _idUser = _preferences.Get(AppConstants.IdUser, 0);
                _contractList = await _contractService.GetContracts(_idUser);
                if (_contractService != null)
                    _contractList.ForEach(Contracts.Add);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}