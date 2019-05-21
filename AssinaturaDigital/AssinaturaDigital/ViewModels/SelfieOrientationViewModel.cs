using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class SelfieOrientationViewModel : ViewModelBase, INavigatingAware
    {
        private bool _isSigningContract;
        private ContractData _contract;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand CloseModalCommand { get; }

        public SelfieOrientationViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            Title = "Orientações Importantes";
            HasBackNavigation = false;
            HasFowardNavigation = false;
            CloseModalCommand = new DelegateCommand(CloseModal, () => !IsBusy)
                .ObservesProperty(() => IsBusy);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.SigningContract))
                _isSigningContract = parameters.GetValue<bool>(AppConstants.SigningContract);

            if (parameters.ContainsKey(AppConstants.Contract))
                _contract = parameters.GetValue<ContractData>(AppConstants.Contract);
        }

        private async void CloseModal()
        {
            try
            {
                IsBusy = true;

                if (_isSigningContract)
                    await _navigationService.NavigateAsync(nameof(SelfiePage),
                        new NavigationParameters
                        {
                            { AppConstants.Contract, _contract },
                            { AppConstants.SigningContract, _isSigningContract }
                        });
                else
                    await _navigationService.NavigateAsync(nameof(SelfiePage));
            }
            catch (Exception ex)
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

