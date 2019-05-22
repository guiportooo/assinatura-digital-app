using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Contracts;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class InfoSigningContractViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IContractsService _contractsService;
        private readonly IPreferences _preferences;

        public DelegateCommand GoHomeCommand { get; }
        public DelegateCommand GoContractListCommand { get; }
        public DelegateCommand GoContractDetailCommand { get; }

        private ContractData _contract;
        public ContractData Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }

        private bool _signed;
        public bool Signed
        {
            get => _signed;
            set => SetProperty(ref _signed, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public InfoSigningContractViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContractsService contractsService,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _contractsService = contractsService;
            _preferences = preferences;

            GoHomeCommand = new DelegateCommand(GoHome).ObservesProperty(() => IsBusy);
            GoContractListCommand = new DelegateCommand(GoToContractsList).ObservesProperty(() => IsBusy);
            GoContractDetailCommand = new DelegateCommand(GoToContract).ObservesProperty(() => IsBusy);
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                GoBack();
                return;
            }

            var idUser = _preferences.Get(AppConstants.IdUser, 0);

            if (idUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");
                GoBack();
                return;
            }

            var selfie = parameters.GetValue<MediaFile>(AppConstants.Selfie);
            Contract = parameters.GetValue<ContractData>(AppConstants.Contract);
            await Initialize(idUser, selfie);
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null
            && parameters[AppConstants.Selfie] != null
            && parameters[AppConstants.Contract] != null;

        async Task Initialize(int idUser, MediaFile photo)
        {
            try
            {
                IsBusy = true;
                Signed = await _contractsService.SignContract(Contract.Id, idUser, photo);

                if (Signed)
                {
                    Contract.Sign();
                    Title = "Contrato assinado!";
                    Message = "";
                    await _pageDialogService.DisplayAlertAsync("Contrato assinado", "Contrato assinado com sucesso!!", "Ok");
                }
                else
                {
                    Title = "Assinatura não realizada!";
                    Message = "Sua Imagem não está compatível com o cadastro.\nPor favor, tire uma nova selfie para assinatura.";
                }
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao validar selfie.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

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

        private async void GoToContract()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(ContractDetailPage),
                    new NavigationParameters
                    {
                        { AppConstants.Contract, Contract }
                    });
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao navegar para detalhes do contrato.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
