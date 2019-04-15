using AssinaturaDigital.Models;
using AssinaturaDigital.Services;
using AssinaturaDigital.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssinaturaDigital.ViewModels
{
    public class TokenViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ITokenService _tokenService;
        private readonly IDeviceTimer _deviceTimer;

        public DelegateCommand GenerateTokenCommand { get; set; }
        public DelegateCommand ValidateTokenCommand { get; }

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

        public TokenViewModel(INavigationService navigationService,
             IPageDialogService pageDialogService, 
             ITokenService tokenService,
             IDeviceTimer deviceTimer)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _tokenService = tokenService;
            _deviceTimer = deviceTimer;

            GenerateTokenCommand = new DelegateCommand(async () => await GenerateToken(), CanGenerateToken)
                .ObservesProperty(() => IsBusy)
                .ObservesProperty(() => SecondsToGenerateToken);

            ValidateTokenCommand = new DelegateCommand(ValidateToken, CanValidateToken)
                .ObservesProperty(() => IsBusy)
                .ObservesProperty(() => TokenDigits);

            Title = "Token";

            TokenDigits = new NotifyCommandCollectionModel<TokenDigit>(ValidateTokenCommand, new List<TokenDigit>
            {
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty),
                new TokenDigit(string.Empty)
            });
        }

        public async void OnNavigatingTo(INavigationParameters parameters) => await GenerateToken();

        async Task GenerateToken()
        {
            var fakeToken = await _tokenService.GenerateToken();
            SecondsToGenerateToken = AppConstants.secondsToGenerateToken;
            StartTimer();
            await _pageDialogService.DisplayAlertAsync(Title, fakeToken, "OK");
        }

        void StartTimer() => _deviceTimer.Start(1, () =>
        {
            SecondsToGenerateToken--;
            return SecondsToGenerateToken > 0;
        });

        bool CanGenerateToken() => !IsBusy && SecondsToGenerateToken == 0;

        bool CanValidateToken() => !IsBusy && !TokenDigits.Items.Any(x => string.IsNullOrEmpty(x.Digit));

        async void ValidateToken()
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

                await _navigationService.NavigateAsync(nameof(UserTermsPage));
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
