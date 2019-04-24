using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AssinaturaDigital.ViewModels
{
    public class TokenViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConfigurationManager _configurationManager;
        private readonly ITokenService _tokenService;
        private readonly IDeviceTimer _deviceTimer;

        public DelegateCommand GenerateTokenCommand { get; }

        private int _secondsToGenerateToken;
        public int SecondsToGenerateToken
        {
            get => _secondsToGenerateToken;
            set => SetProperty(ref _secondsToGenerateToken, value);
        }

        private NotifyCommandCollectionModel<TokenDigit> _tokenDigits;
        public NotifyCommandCollectionModel<TokenDigit> TokenDigits
        {
            get => _tokenDigits;
            set => SetProperty(ref _tokenDigits, value);
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


        public TokenViewModel(INavigationService navigationService,
             IPageDialogService pageDialogService,
             IConfigurationManager configurationManager,
             ITokenService tokenService,
             IDeviceTimer deviceTimer) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _configurationManager = configurationManager;
            _tokenService = tokenService;
            _deviceTimer = deviceTimer;

            GenerateTokenCommand = new DelegateCommand(async () => await GenerateToken(), CanGenerateToken)
                .ObservesProperty(() => IsBusy)
                .ObservesProperty(() => SecondsToGenerateToken);

            Title = "Token";

            ClearTokenDigits();
            InitializeSteps();
        }

        void InitializeSteps()
        {
            CurrentStep = 2;
            StepsList = new ObservableCollection<Steps> {
                new Steps(true),
                new Steps(true),
                new Steps(false),
                new Steps(false),
                new Steps(false),
            };
        }

        void ClearTokenDigits() =>
            TokenDigits = new NotifyCommandCollectionModel<TokenDigit>(GoFowardCommand, new List<TokenDigit>
            {
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty)
            });

        public async void OnNavigatedTo(INavigationParameters parameters) => await Initialize();

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        async Task Initialize()
        {
            ClearTokenDigits();
            await GenerateToken();
        }

        async Task GenerateToken()
        {
            var fakeToken = await _tokenService.GenerateToken();

            var config = _configurationManager.Get();
            SecondsToGenerateToken = config.SecondsToGenerateToken;

            StartTimer();

            await _pageDialogService.DisplayAlertAsync(Title, fakeToken, "OK");
        }

        void StartTimer() => _deviceTimer.Start(1, () =>
        {
            SecondsToGenerateToken--;
            return SecondsToGenerateToken > 0;
        });

        bool CanGenerateToken() => !IsBusy && SecondsToGenerateToken == 0;

        protected override bool CanGoFoward() => !IsBusy && !TokenDigits.Items.Any(x => string.IsNullOrEmpty(x.Digit));

        protected override async void GoFoward()
        {
            try
            {
                IsBusy = true;

                var token = string.Join(string.Empty, TokenDigits.Items.Select(x => x.Digit));
                var tokenIsValid = await _tokenService.ValidateToken(token);

                if (!tokenIsValid)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Token inválido!", "OK");
                    return;
                }

                await _navigationService.NavigateAsync(nameof(TermsOfUsePage));
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
