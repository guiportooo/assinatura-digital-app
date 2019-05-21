using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class CNHOrientationViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand CloseModalCommand { get; }

        public CNHOrientationViewModel(INavigationService navigationService,
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

        private async void CloseModal()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(DocumentPage),
                    new NavigationParameters
                    {
                        { AppConstants.DocumentType, DocumentType.CNH }
                    });
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

