using AssinaturaDigital.Models;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;

namespace AssinaturaDigital.ViewModels
{
    public class DocumentsSelectionViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand ShowInfoCommand { get; }
        public DelegateCommand ChooseRGPictureCommand { get; }
        public DelegateCommand ChooseCNHPictureCommand { get; }

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

        public DocumentsSelectionViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            ShowInfoCommand = new DelegateCommand(ShowInfo);
            ChooseRGPictureCommand = new DelegateCommand(ChooseRGPicture, CanChooseDocument)
                .ObservesProperty(() => IsBusy);
            ChooseCNHPictureCommand = new DelegateCommand(ChooseCNHPicture, CanChooseDocument)
                .ObservesProperty(() => IsBusy);

            InitializeSteps();

            Title = "Documento";
            HasFowardNavigation = false;
        }

        void InitializeSteps()
        {
            CurrentStep = 4;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(true),
                new Steps(false),
            };
        }

        async void ShowInfo() 
            => await _pageDialogService.DisplayAlertAsync(Title, 
                "Selecione o documento de sua preferência para a validação do seu cadastro.\nLembre-se de tirar 2 fotos: frente e verso do documento, sempre na orientação vertical.", 
                "OK");

        bool CanChooseDocument() => !IsBusy;

        async void ChooseCNHPicture()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(CNHOrientationPage), useModalNavigation: true, animated: true);
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

        async void ChooseRGPicture()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(RGOrientationPage), useModalNavigation: true, animated: true);
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
