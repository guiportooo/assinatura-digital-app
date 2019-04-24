using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AssinaturaDigital.ViewModels
{
    public class DocumentsSelectionViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

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

        bool CanChooseDocument() => !IsBusy;

        async void ChooseCNHPicture()
        {
            try
            {
                IsBusy = true;
                await NavigateToDocumentPage(AppConstants.CNH);
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
                await NavigateToDocumentPage(AppConstants.RG);
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

        async Task NavigateToDocumentPage(string documentType)
            => await _navigationService.NavigateAsync(nameof(DocumentPage), 
                new NavigationParameters
                {
                    { AppConstants.DocumentType, documentType }
                });
    }
}
