using AssinaturaDigital.Events;
using AssinaturaDigital.Services;
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
        private readonly ITermsOfUseServices _userTermsServices;
        private readonly IPageDialogService _pageDialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;

        public DelegateCommand AcceptTermsCommand { get; }

        private string _termsUse;
        public string TermsUse
        {
            get => _termsUse;
            set => SetProperty(ref _termsUse, value);
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

        public TermsOfUseViewModel(ITermsOfUseServices userTermsServices,
            IPageDialogService pageDialogService,
            IEventAggregator eventAggregator,
            INavigationService navigationService)
        {
            Title = "Termos de uso";
            TermsUse = string.Empty;

            _userTermsServices = userTermsServices;
            _pageDialogService = pageDialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            AcceptTermsCommand = new DelegateCommand(AcceptTerms, CanAcceptTerms)
                .ObservesProperty(() => AcceptedTerms);
        }

        bool CanAcceptTerms() => AcceptedTerms;

        async void AcceptTerms() => 
                await _navigationService.NavigateAsync(nameof(DocumentsPage));

        void EndOfScroll() =>
            ReadTerms = true;

        public async void OnNavigatingTo(INavigationParameters parameters) =>
            TermsUse = await _userTermsServices.GetTermsUse();

        public void OnNavigatedFrom(INavigationParameters parameters) =>
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Unsubscribe(EndOfScroll);

        public void OnNavigatedTo(INavigationParameters parameters) =>
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Subscribe(EndOfScroll);
    }
}
