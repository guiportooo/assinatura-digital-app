using AssinaturaDigital.Events;
using AssinaturaDigital.Services;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace AssinaturaDigital.ViewModels
{
    public class TermsOfUseViewModel : ViewModelBase, INavigatingAware, INavigatedAware
    {
        private readonly ITermsOfUseServices _userTermsServices;
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;

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

        public TermsOfUseViewModel(ITermsOfUseServices userTermsServices,
            IEventAggregator eventAggregator,
            INavigationService navigationService)
        {
            Title = "Termos de uso";

            _userTermsServices = userTermsServices;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            AcceptTermsCommand = new DelegateCommand(AcceptTerms, CanAcceptTerms)
                .ObservesProperty(() => AcceptedTerms);
        }

        bool CanAcceptTerms() => AcceptedTerms;

        async void AcceptTerms()
            => await _navigationService.NavigateAsync(nameof(DocumentsPage));

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
