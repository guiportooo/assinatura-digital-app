using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SignInViewModelTests
    {
        private SignInViewModel _signInViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private AuthenticationServiceFake _authenticationService;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _authenticationService = new AuthenticationServiceFake();
            _authenticationService.ShouldDelay(false);
            _preferencesMock = new Mock<IPreferences>();
            _signInViewModel = new SignInViewModel(_navigationService, _pageDialogService, _authenticationService, _preferencesMock.Object);
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitle() => _signInViewModel.Title.Should().Be("Confirmação");

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void WhenPageIsBusyShouldNotBeAbleToSignIn(bool pageIsBusy, bool canSignUp)
        {
            _signInViewModel.IsBusy = pageIsBusy;
            _signInViewModel.GoFowardCommand.CanExecute().Should().Be(canSignUp);
        }

        [Test]
        public void ShouldDisplayErrorMessageWhenCanNotGetUserByCpf()
        {
            const string message = "CPF não cadastrado. Por favor, acesse a página inicial e clique em 1º acesso.";
            const string cancelButton = "OK";

            _signInViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_signInViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }

        [Test]
        public void WhenSigningInShouldSaveIdUserOnPreferences()
        {
            const string cpf = "123.456.789-10";
            _authenticationService.ShouldReturnUseWithCpf(cpf);
            _signInViewModel.CPF = cpf;

            _signInViewModel.GoFowardCommand.Execute();

            _preferencesMock.Verify(x => x.Set(AppConstants.IdUser, _authenticationService.ReturningUser.Id));
        }

        [Test]
        public void WhenSigningInShouldNavigateToTokenPage()
        {
            const string cpf = "123.456.789-10";
            _authenticationService.ShouldReturnUseWithCpf(cpf);
            _signInViewModel.CPF = cpf;
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.Registered, true }
            };

            _signInViewModel.GoFowardCommand.Execute();
            _navigationService.Name.Should().Be(nameof(TokenPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void AfterSigningInShouldSetIsBusyToFalse()
        {
            _signInViewModel.GoFowardCommand.Execute();
            _signInViewModel.IsBusy.Should().BeFalse();
        }

        [Test]
        public void WhenFailingToSigningInShouldDisplayAlertWithErrorMessage()
        {
            const string message = "Falha ao validar usuário.";
            const string cancelButton = "OK";

            _authenticationService.ShouldFail(true);

            _signInViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_signInViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }
    }
}
