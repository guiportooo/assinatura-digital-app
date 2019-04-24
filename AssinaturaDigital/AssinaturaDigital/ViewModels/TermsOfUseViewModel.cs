using AssinaturaDigital.Events;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Views;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssinaturaDigital.ViewModels
{
    public class TermsOfUseViewModel : ViewModelBase, INavigatingAware, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ITermsOfUseServices _userTermsServices;
        private readonly IEventAggregator _eventAggregator;

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

        private ObservableCollection<Steps> _steps;
        public ObservableCollection<Steps> StepsList
        {
            get => _steps;
            set => SetProperty(ref _steps, value);
        }

        private int _currentStep;
        public int CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }

        public TermsOfUseViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ITermsOfUseServices userTermsServices,
            IEventAggregator eventAggregator) : base(navigationService, pageDialogService)
        {
            Title = "Termos de uso";

            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _userTermsServices = userTermsServices;
            _eventAggregator = eventAggregator;

            InitializeSteps();
        }

        void InitializeSteps()
        {
            CurrentStep = 3;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(false),
                new Steps(false),
            };
        }

        protected override bool CanGoFoward() => !IsBusy && AcceptedTerms;

        protected override async void GoFoward()
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
