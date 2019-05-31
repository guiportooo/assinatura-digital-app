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
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SignUpViewModelTests
    {
        private readonly string _fullName = "Test User";
        private readonly string _cpf = "222.222.222-22";
        private readonly string _cellPhoneNumber = "987654321";
        private readonly string _email = "test.user@appsign.com";

        private SignUpViewModel _signUpViewModel;
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
            _signUpViewModel = new SignUpViewModel(_navigationService, _pageDialogService, _authenticationService, _preferencesMock.Object);
        }

        [Test]
        public void WhenNavigatingToPageShouldConfigureSteps()
        {
            var expectedCurrentStep = 1;
            var expectedCountListSteps = 5;
            _signUpViewModel.CurrentStep.Should().Be(expectedCurrentStep);
            _signUpViewModel.StepsList.Count.Should().Be(expectedCountListSteps);
            _signUpViewModel.StepsList[0].Done.Should().BeTrue();
            _signUpViewModel.StepsList[1].Done.Should().BeFalse();
            _signUpViewModel.StepsList[2].Done.Should().BeFalse();
            _signUpViewModel.StepsList[3].Done.Should().BeFalse();
            _signUpViewModel.StepsList[4].Done.Should().BeFalse();
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitle() => _signUpViewModel.Title.Should().Be("Cadastro");

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void WhenPageIsBusyShouldNotBeAbleToSignUp(bool pageIsBusy, bool canSignUp)
        {
            _signUpViewModel.FullName = _fullName;
            _signUpViewModel.CPF = _cpf;
            _signUpViewModel.CellPhoneNumber = _cellPhoneNumber;
            _signUpViewModel.Email = _email;

            _signUpViewModel.IsBusy = pageIsBusy;
            _signUpViewModel.SignUpCommand.CanExecute().Should().Be(canSignUp);
        }

        [TestCase("", "", "", "", false)]
        [TestCase("FullName", "", "", "", false)]
        [TestCase("FullName", "CPF", "", "", false)]
        [TestCase("FullName", "CPF", "CellPhoneNumber", "", false)]
        [TestCase("FullName", "CPF", "CellPhoneNumber", "Email", true)]
        public void WhenDidNotFilledUpFormShouldNotBeAbleToSignUp(string fullName, string cpf, string cellPhoneNumber, string email, bool canSignUp)
        {
            _signUpViewModel.IsBusy = false;

            _signUpViewModel.FullName = fullName;
            _signUpViewModel.CPF = cpf;
            _signUpViewModel.CellPhoneNumber = cellPhoneNumber;
            _signUpViewModel.Email = email;
            _signUpViewModel.SignUpCommand.CanExecute().Should().Be(canSignUp);
        }

        [Test]
        public void WhenSigningUpShouldSendInformationToTheService()
        {
            _signUpViewModel.FullName = _fullName;
            _signUpViewModel.CPF = _cpf;
            _signUpViewModel.CellPhoneNumber = _cellPhoneNumber;
            _signUpViewModel.Email = _email;

            var expectedSignUpInformation = new SignUpInformation(_fullName, _cpf, _cellPhoneNumber, _email);

            _signUpViewModel.SignUpCommand.Execute();

            _authenticationService.SignUpInformation.Should().BeEquivalentTo(expectedSignUpInformation);
        }

        [Test]
        public void WhenSigningUpShouldSaveIdUserOnPreferences()
        {
            _signUpViewModel.SignUpCommand.Execute();
            _preferencesMock.Verify(x => x.Set(AppConstants.IdUser, _authenticationService.ReturningUser.Id));
        }

        [Test]
        public void WhenSigningUpShouldNavigateToTokenPage()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.Registered, false }
            };

            _signUpViewModel.SignUpCommand.Execute();
            _navigationService.Name.Should().Be(nameof(TokenPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void AfterSigningUpShouldSetIsBusyToFalse()
        {
            _signUpViewModel.SignUpCommand.Execute();
            _signUpViewModel.IsBusy.Should().BeFalse();
        }

        [Test]
        public void WhenSingingUpWithAnExistingCPFShouldDisplayAlertWithErrorMessage()
        {
            const string message = "CPF já cadastrado.";
            const string cancelButton = "OK";

            _signUpViewModel.FullName = _fullName;
            _signUpViewModel.CPF = _cpf;
            _signUpViewModel.CellPhoneNumber = _cellPhoneNumber;
            _signUpViewModel.Email = _email;

            _authenticationService.ShouldValidateExistingCpf(true);

            _signUpViewModel.SignUpCommand.Execute();

            _pageDialogService.Title.Should().Be(_signUpViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }

        [Test]
        public void WhenFailingToSigningUpShouldDisplayAlertWithErrorMessage()
        {
            const string message = "Falha ao cadastrar usuário.";
            const string cancelButton = "OK";

            _authenticationService.ShouldFail(true);

            _signUpViewModel.SignUpCommand.Execute();

            _pageDialogService.Title.Should().Be(_signUpViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }
    }
}
