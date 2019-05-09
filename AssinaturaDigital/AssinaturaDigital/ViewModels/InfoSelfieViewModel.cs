using AssinaturaDigital.Models;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;

namespace AssinaturaDigital.ViewModels
{
    public class InfoSelfieViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand ShowInfoCommand { get; }

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

        public InfoSelfieViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            ShowInfoCommand = new DelegateCommand(ShowInfo);

            InitializeSteps();

            Title = "Info Selfie";
        }

        void InitializeSteps()
        {
            CurrentStep = 5;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(true),
            };
        }

        async void ShowInfo()
        {
            var info = "Para tornar a assinatura do contrato mais segura e possibilitar maior praticidade no processo, transformaremos sua foto em um algoritmo biométrico para identificação.";
            await _pageDialogService.DisplayAlertAsync(Title, info, "OK");
        }

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(SelfiePage));
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao avançar para tela de selfie.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
