using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssinaturaDigital.ViewModels
{
    public class DocumentsViewModel : ViewModelBase    
    {
        INavigationService _navigationService;
        public DelegateCommand ChooseRGPictureCommand { get; }
        public DelegateCommand ChooseCNHPictureCommand { get; }

        public DocumentsViewModel(INavigationService navigationService)
        {
            Title = "Documento";

            _navigationService = navigationService;

            ChooseRGPictureCommand = new DelegateCommand(ChooseRGPicture);
            ChooseCNHPictureCommand = new DelegateCommand(ChooseCNHPicture);
        }

        async void ChooseCNHPicture() =>
            await _navigationService.NavigateAsync(nameof(CNHPage));

        async void ChooseRGPicture() =>
            await _navigationService.NavigateAsync(nameof(RGPage));
    }
}
