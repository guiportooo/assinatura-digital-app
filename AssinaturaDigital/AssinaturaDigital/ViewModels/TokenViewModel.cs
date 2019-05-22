using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Services.Token;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class TokenViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConfigurationManager _configurationManager;
        private readonly ITokenService _tokenService;
        private readonly IDeviceTimer _deviceTimer;
        private readonly IPreferences _preferences;
        private int _idUser;

        public DelegateCommand ShowInfoCommand { get; }
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

        private bool _registered;
        public bool Registered
        {
            get => _registered;
            set => SetProperty(ref _registered, value);
        }

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        public TokenViewModel(INavigationService navigationService,
             IPageDialogService pageDialogService,
             IConfigurationManager configurationManager,
             ITokenService tokenService,
             IDeviceTimer deviceTimer,
             IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _configurationManager = configurationManager;
            _tokenService = tokenService;
            _deviceTimer = deviceTimer;
            _preferences = preferences;

            ShowInfoCommand = new DelegateCommand(ShowInfo);
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

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.Registered))
                Registered = parameters.GetValue<bool>(AppConstants.Registered);

            await Initialize();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        async Task Initialize()
        {
            ClearTokenDigits();
            _idUser = _preferences.Get(AppConstants.IdUser, 0);

            if (_idUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");

                await _navigationService.NavigateAsync(nameof(MainPage));
                return;
            }
            await GenerateToken();
        }

        async void ShowInfo() 
            => await _pageDialogService.DisplayAlertAsync(Title, 
                "Para maior segurança de acesso, um token de 6 dígitos será enviado para sua autenticação.", 
                "OK");

        async Task GenerateToken()
        {
            try
            {
                IsBusy = true;
                var response = await _tokenService.GenerateToken(_idUser);

                if (!response.Succeeded)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Falha ao enviar token.", "OK");
                    GoBack();
                }

                var config = _configurationManager.Get();
                SecondsToGenerateToken = config.SecondsToGenerateToken;

                StartTimer();

                if (config.UseFakes)
                {
                    var fakeToken = ((TokenResponseFake)response).Token;
                    await _pageDialogService.DisplayAlertAsync(Title, fakeToken, "OK");
                }
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao enviar token.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        void StartTimer() => _deviceTimer.Start(1, () =>
        {
            SecondsToGenerateToken--;
            return SecondsToGenerateToken > 0;
        });

        bool CanGenerateToken() => !IsBusy && SecondsToGenerateToken == 0;

        bool AllDigitsWereInformed() => !TokenDigits.Items.Any(x => string.IsNullOrEmpty(x.Digit.Trim()));

        protected override bool CanGoFoward() => !IsBusy && AllDigitsWereInformed();

        protected override async void GoFoward()
        {
            try
            {
                if (!CanGoFoward())
                    return;

                IsBusy = true;

                var token = string.Join(string.Empty, TokenDigits.Items.Select(x => x.Digit)).Substring(0,TokenDigits.Items.Count);
                var tokenIsValid = await _tokenService.ValidateToken(_idUser, token);

                if (!tokenIsValid)
                {
                    await _pageDialogService.DisplayAlertAsync(Title, "Token inválido!", "OK");
                    return;
                }

                if (!Registered)
                {
                    await _navigationService.NavigateAsync(nameof(TermsOfUsePage));
                    return;
                }

                await _navigationService.NavigateAsync(nameof(HomePage));
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao validar token.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
