using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class TokenViewModelTests
    {
        private readonly int _idUser = 1;
        private TokenViewModel _tokenViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private ConfigurationManagerMock _configurationManager;
        private TokenServiceFake _tokenService;
        private DeviceTimerFake _deviceTimer;

        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _configurationManager = new ConfigurationManagerMock();
            _tokenService = new TokenServiceFake();
            _deviceTimer = new DeviceTimerFake();
            _preferencesMock = new Mock<IPreferences>();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _tokenViewModel = new TokenViewModel(_navigationService,
                _pageDialogService,
                _configurationManager,
                _tokenService,
                _deviceTimer,
                _preferencesMock.Object);
        }

        void SetTokenDigits(ObservableCollection<TokenDigit> tokenDigits)
            => _tokenViewModel.TokenDigits = new NotifyCommandCollectionModel<TokenDigit>(_tokenViewModel.GoFowardCommand,
                tokenDigits);

        [Test]
        public void WhenNavigatingToPageShouldConfigureSteps()
        {
            var expectedCurrentStep = 2;
            var expectedCountListSteps = 5;
            _tokenViewModel.CurrentStep.Should().Be(expectedCurrentStep);
            _tokenViewModel.StepsList.Count.Should().Be(expectedCountListSteps);
            _tokenViewModel.StepsList[0].Done.Should().BeTrue();
            _tokenViewModel.StepsList[1].Done.Should().BeTrue();
            _tokenViewModel.StepsList[2].Done.Should().BeFalse();
            _tokenViewModel.StepsList[3].Done.Should().BeFalse();
            _tokenViewModel.StepsList[4].Done.Should().BeFalse();
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitleAndTokenDigits()
        {
            var expectedTokenDigits = new NotifyCommandCollectionModel<TokenDigit>(_tokenViewModel.GoFowardCommand,
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
        public void WhenExecuteInfoSelfieCommandShouldGetInfosSelfie()
        {
            var expectedInfos = "Para maior segurança de acesso, um token de 6 dígitos será enviado para sua autenticação.";
            _tokenViewModel.ShowInfoCommand.Execute();
            _pageDialogService.Message.Should().Be(expectedInfos);
        }

        [Test]
        public void WhenNavigatingToPageShouldSetSecondsToGenerateTokenAndStartTheTimer()
        {
            var expectedSecondsToGenerateToken = _configurationManager.Get().SecondsToGenerateToken;
            _tokenViewModel.OnNavigatedTo(new NavigationParameters());
            _tokenViewModel.SecondsToGenerateToken.Should().Be(expectedSecondsToGenerateToken);
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
            var expectedSecondsToGenerateToken = _configurationManager.Get().SecondsToGenerateToken;
            _tokenViewModel.GenerateTokenCommand.Execute();
            _tokenViewModel.SecondsToGenerateToken.Should().Be(expectedSecondsToGenerateToken);
            _deviceTimer.Seconds.Should().Be(1);
            _deviceTimer.Callback.Should().NotBeNull();
        }

        [Test]
        public void TimerShouldDecrementSecondsToGenerateTokenEverySecond()
        {
            var secondsToGenerateToken = _configurationManager.Get().SecondsToGenerateToken;

            var expectedSecondsToGenerateToken = secondsToGenerateToken;

            _tokenViewModel.GenerateTokenCommand.Execute();
            _tokenViewModel.SecondsToGenerateToken.Should().Be(expectedSecondsToGenerateToken);

            _deviceTimer.Callback.Invoke();
            expectedSecondsToGenerateToken = secondsToGenerateToken - 1;

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

            _tokenViewModel.GoFowardCommand.CanExecute().Should().Be(true);
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

            _tokenViewModel.GoFowardCommand.CanExecute().Should().Be(false);
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
            _tokenViewModel.GoFowardCommand.CanExecute().Should().Be(canValidateToken);
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

            _tokenViewModel.GoFowardCommand.Execute();

            _tokenService.PassedToken.Should().Be("123456");
        }

        [Test]
        public async Task WhenValidatingWithAValidTokenShouldNavigateToUserTermsPage()
        {
            var tokenResponse = await _tokenService.GenerateToken(_idUser);
            var validToken = ((TokenResponseFake)tokenResponse).Token;

            SetTokenDigits(validToken
                .ToCharArray()
                .Select(x => new TokenDigit(x.ToString()))
                .ToObservableCollection());

            _tokenViewModel.GoFowardCommand.Execute();

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

            _tokenViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_tokenViewModel.Title);
            _pageDialogService.Message.Should().Be("Token inválido!");
            _pageDialogService.CancelButton.Should().Be("OK");
        }

        [Test]
        public void WhenRaisingAnExceptionOnValidatingTokenShouldDisplayErrorMessage()
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
            const string exceptionMessage = "Falha ao validar token.";
            _tokenService.ShouldRaiseException(new Exception(exceptionMessage));

            _tokenViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_tokenViewModel.Title);
            _pageDialogService.Message.Should().Be(exceptionMessage);
            _pageDialogService.CancelButton.Should().Be("OK");
        }
    }
}
