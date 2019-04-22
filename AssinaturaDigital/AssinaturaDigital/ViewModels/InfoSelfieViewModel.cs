using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class InfoSelfieViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand TakeSelfieCommand { get; }
        public DelegateCommand ShowInfoCommand { get; }

        public InfoSelfieViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            ShowInfoCommand = new DelegateCommand(ShowInfo);
            TakeSelfieCommand = new DelegateCommand(TakeSelfie, CanTakeSelfie)
                .ObservesProperty(() => IsBusy);

            Title = "Info Selfie";
        }

        async void ShowInfo()
        {
            var info = "Para tornar a assinatura do contrato mais segura e possibilitar maior praticidade no processo, transformaremos sua foto em um algoritmo biométrico para identificação.";
            await _pageDialogService.DisplayAlertAsync(Title, info, "OK");
        }

        async void TakeSelfie()
        {
            try
            {
                IsBusy = true;
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

        bool CanTakeSelfie() => !IsBusy;
    }
}
