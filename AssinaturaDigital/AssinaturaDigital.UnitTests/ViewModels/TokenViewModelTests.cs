using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class TokenViewModelTests
    {
        private string _validToken;
        private TokenViewModel _tokenViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private TokenServiceFake _tokenService;
        private DeviceTimerFake _deviceTimer;

        [SetUp]
        public async Task Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _tokenService = new TokenServiceFake();
            _deviceTimer = new DeviceTimerFake();

            _validToken = await _tokenService.GenerateToken();

            _tokenViewModel = new TokenViewModel(_navigationService,
                _pageDialogService,
                _tokenService,
                _deviceTimer);
        }

        void SetTokenDigits(ObservableCollection<TokenDigit> tokenDigits)
            => _tokenViewModel.TokenDigits = new NotifyCommandCollectionModel<TokenDigit>(_tokenViewModel.ValidateTokenCommand,
                tokenDigits);

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitleAndTokenDigits()
        {
            var expectedTokenDigits = new NotifyCommandCollectionModel<TokenDigit>(_tokenViewModel.ValidateTokenCommand,
                new ObservableCollection<TokenDigit>
                {
                    new TokenDigit(string.Empty),
                    new TokenDigit(string.Empty),
                    new TokenDigit(string.Empty),
                    new TokenDigit(string.Empty),
                    new TokenDigit(string.Empty),
                    new TokenDigit(string.Empty)
                });

            _tokenViewModel.Title.Should().Be("Token");
            _tokenViewModel.SecondsToGenerateToken.Should().Be(0);
            _tokenViewModel.TokenDigits.Should().BeEquivalentTo(expectedTokenDigits);
        }

        [Test]
        public void WhenNavigatingToPageShouldSetSecondsToGenerateTokenAndStartTheTimer()
        {
            _tokenViewModel.OnNavigatingTo(new NavigationParameters());
            _tokenViewModel.SecondsToGenerateToken.Should().Be(AppConstants.secondsToGenerateToken);
            _deviceTimer.Seconds.Should().Be(1);
            _deviceTimer.Callback.Should().NotBeNull();
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void WhenPageIsBusyShouldNotBeAbleToGenerateToken(bool pageIsBusy, bool canValidateToken)
        {
            _tokenViewModel.IsBusy = pageIsBusy;
            _tokenViewModel.GenerateTokenCommand.CanExecute().Should().Be(canValidateToken);
        }

        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(60, false)]
        [TestCase(-1, false)]
        public void WhenSecondsToGenerateTokenIsDifferentThanZeroShouldNotBeAbleToGenerateToken(int secondsToGenerateToken, bool canValidateToken)
        {
            _tokenViewModel.SecondsToGenerateToken = secondsToGenerateToken;
            _tokenViewModel.GenerateTokenCommand.CanExecute().Should().Be(canValidateToken);
        }

        [Test]
        public void WhenGeneratingTokenShouldResetSecondsToGenerateTokenAndStartTheTimer()
        {
            _tokenViewModel.GenerateTokenCommand.Execute();
            _tokenViewModel.SecondsToGenerateToken.Should().Be(AppConstants.secondsToGenerateToken);
            _deviceTimer.Seconds.Should().Be(1);
            _deviceTimer.Callback.Should().NotBeNull();
        }

        [Test]
        public void TimerShouldDecrementSecondsToGenerateTokenEverySecond()
        {
            var expectedSecondsToGenerateToken = AppConstants.secondsToGenerateToken;

            _tokenViewModel.GenerateTokenCommand.Execute();
            _tokenViewModel.SecondsToGenerateToken.Should().Be(expectedSecondsToGenerateToken);

            _deviceTimer.Callback.Invoke();
            expectedSecondsToGenerateToken = AppConstants.secondsToGenerateToken - 1;

            _tokenViewModel.SecondsToGenerateToken.Should().Be(expectedSecondsToGenerateToken);
        }

        [TestCase(60, true)]
        [TestCase(2, true)]
        [TestCase(1, false)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public void TimerShouldStopWhenSecondsToGenerateTokenIsEqualOrLessThanZero(int secondsToGenerateToken, bool shouldRunTimer)
        {
            _tokenViewModel.GenerateTokenCommand.Execute();
            _tokenViewModel.SecondsToGenerateToken = secondsToGenerateToken;
            var timerIsRunning = _deviceTimer.Callback.Invoke();
            timerIsRunning.Should().Be(shouldRunTimer);
        }

        [Test]
        public void WhenAllTokenDigitsAreFilledShouldBeAbleToValidateToken()
        {
            SetTokenDigits(new ObservableCollection<TokenDigit>
            {
                new TokenDigit("1"),
                new TokenDigit("2"),
                new TokenDigit("3"),
                new TokenDigit("4"),
                new TokenDigit("5"),
                new TokenDigit("6")
            });

            _tokenViewModel.ValidateTokenCommand.CanExecute().Should().Be(true);
        }

        [Test]
        public void WhenAtLeastOneOfTheTokenDigitsIsEmptyShouldNotBeAbleToValidateToken()
        {
            SetTokenDigits(new ObservableCollection<TokenDigit>
            {
                new TokenDigit("1"),
                new TokenDigit("2"),
                new TokenDigit("3"),
                new TokenDigit("4"),
                new TokenDigit("5"),
                new TokenDigit("")
            });

            _tokenViewModel.ValidateTokenCommand.CanExecute().Should().Be(false);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void WhenPageIsBusyShouldNotBeAbleToValidateToken(bool pageIsBusy, bool canValidateToken)
        {
            SetTokenDigits(new ObservableCollection<TokenDigit>
            {
                new TokenDigit("1"),
                new TokenDigit("2"),
                new TokenDigit("3"),
                new TokenDigit("4"),
                new TokenDigit("5"),
                new TokenDigit("6")
            });

            _tokenViewModel.IsBusy = pageIsBusy;
            _tokenViewModel.ValidateTokenCommand.CanExecute().Should().Be(canValidateToken);
        }

        [Test]
        public void WhenValidatingTokenShouldSendJoinedTokenDigits()
        {
            SetTokenDigits(new ObservableCollection<TokenDigit>
            {
                new TokenDigit("1"),
                new TokenDigit("2"),
                new TokenDigit("3"),
                new TokenDigit("4"),
                new TokenDigit("5"),
                new TokenDigit("6")
            });

            _tokenViewModel.ValidateTokenCommand.Execute();

            _tokenService.PassedToken.Should().Be("123456");
        }

        [Test]
        public void WhenValidatingWithAValidTokenShouldNavigateToUserTermsPage()
        {
            SetTokenDigits(_validToken
                .ToCharArray()
                .Select(x => new TokenDigit(x.ToString()))
                .ToObservableCollection());

            _tokenViewModel.ValidateTokenCommand.Execute();

            _navigationService.Name.Should().Be(nameof(TermsOfUsePage));
        }

        [Test]
        public void WhenValidatingWithAnInvalidTokenShouldDisplayErrorMessage()
        {
            var invalidToken = "111111";
            SetTokenDigits(invalidToken
                .ToCharArray()
                .Select(x => new TokenDigit(x.ToString()))
                .ToObservableCollection());

            _tokenViewModel.ValidateTokenCommand.Execute();

            _pageDialogService.Title.Should().Be(_tokenViewModel.Title);
            _pageDialogService.Message.Should().Be("Token inválido!");
            _pageDialogService.CancelButton.Should().Be("OK");
        }

        [Test]
        public void WhenRaisingAnExceptionOnValidatingTokenShouldDisplayErrorMessage()
        {
            const string exceptionMessage = "Invalid token!";
            _tokenService.ShouldRaiseException(new Exception(exceptionMessage));

            _tokenViewModel.ValidateTokenCommand.Execute();

            _pageDialogService.Title.Should().Be(_tokenViewModel.Title);
            _pageDialogService.Message.Should().Be(exceptionMessage);
            _pageDialogService.CancelButton.Should().Be("OK");
        }
    }
}
