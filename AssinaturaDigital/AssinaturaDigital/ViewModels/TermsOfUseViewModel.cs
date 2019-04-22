using AssinaturaDigital.Events;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public class TermsOfUseViewModel : ViewModelBase, INavigatingAware, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ITermsOfUseServices _userTermsServices;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand AcceptTermsCommand { get; }

        private string _termsOfUse;
        public string TermsOfUse
        {
            get => _termsOfUse;
            set => SetProperty(ref _termsOfUse, value);
        }

        private bool _readTerms;
        public bool ReadTerms
        {
            get => _readTerms;
            set => SetProperty(ref _readTerms, value);
        }

        private bool _acceptedTerms;
        public bool AcceptedTerms
        {
            get => _acceptedTerms;
            set => SetProperty(ref _acceptedTerms, value);
        }

        public TermsOfUseViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ITermsOfUseServices userTermsServices,
            IEventAggregator eventAggregator)
        {
            Title = "Termos de uso";

            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _userTermsServices = userTermsServices;
            _eventAggregator = eventAggregator;

            AcceptTermsCommand = new DelegateCommand(AcceptTerms, CanAcceptTerms)
                .ObservesProperty(() => IsBusy)
                .ObservesProperty(() => AcceptedTerms);
        }

        bool CanAcceptTerms() => !IsBusy && AcceptedTerms;

        async void AcceptTerms()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(DocumentsSelectionPage));
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

        void EndOfScroll() =>
            ReadTerms = true;

        public async void OnNavigatingTo(INavigationParameters parameters) =>
            TermsOfUse = await _userTermsServices.GetTermsUse();

        public void OnNavigatedFrom(INavigationParameters parameters) =>
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Unsubscribe(EndOfScroll);

        public void OnNavigatedTo(INavigationParameters parameters) =>
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Subscribe(EndOfScroll);
    }
}
