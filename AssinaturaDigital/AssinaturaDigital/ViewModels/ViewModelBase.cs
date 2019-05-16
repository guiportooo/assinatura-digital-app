using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;

namespace AssinaturaDigital.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoFowardCommand { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _hasFowardNavigation;
        public bool HasFowardNavigation
        {
            get => _hasFowardNavigation;
            set => SetProperty(ref _hasFowardNavigation, value);
        }

        private bool _hasBackNavigation;
        public bool HasBackNavigation
        {
            get => _hasBackNavigation;
            set => SetProperty(ref _hasBackNavigation, value);
        }

        protected ViewModelBase(INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            HasFowardNavigation = true;
            HasBackNavigation = true;

            GoFowardCommand = new DelegateCommand(GoFoward, CanGoFoward)
                .ObservesProperty(() => IsBusy);

            GoBackCommand = new DelegateCommand(GoBack, CanGoBack)
                .ObservesProperty(() => IsBusy);
        }

        protected async void GoBack()
        {
            try
            {
                IsBusy = true;
                await _navigationService.GoBackAsync();
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

        protected virtual void GoFoward() { }

        protected async void GoHome()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(HomePage));
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

        protected virtual bool CanGoBack() => !IsBusy;

        protected virtual bool CanGoFoward() => !IsBusy;

        protected virtual bool CanGoHome() => !IsBusy;
    }
}
