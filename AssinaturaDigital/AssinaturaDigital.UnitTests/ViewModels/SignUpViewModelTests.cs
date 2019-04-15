using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SignUpViewModelTests
    {
        const string fullName = "Test User";
        const string cpf = "22222222222";
        const string cellphoneNumber = "987654321";
        const string email = "test.user@appsign.com";

        private SignUpViewModel _signUpViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private SignUpServiceFake _signUpService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _signUpService = new SignUpServiceFake();
            _signUpService.ShouldDelay(false);
            _signUpViewModel = new SignUpViewModel(_navigationService, _pageDialogService, _signUpService);
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitle() => _signUpViewModel.Title.Should().Be("Cadastro");

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void WhenPageIsBusyShouldNotBeAbleToSignUp(bool pageIsBusy, bool canSignUp)
        {
            _signUpViewModel.IsBusy = pageIsBusy;
            _signUpViewModel.SignUpCommand.CanExecute().Should().Be(canSignUp);
        }

        [Test]
        public void WhenSigningUpShouldSendInformationToTheService()
        {
            _signUpViewModel.FullName = fullName;
            _signUpViewModel.CPF = cpf;
            _signUpViewModel.CellphoneNumber = cellphoneNumber;
            _signUpViewModel.Email = email;

            var expectedSignUpInformation = new SignUpInformation(fullName, cpf, cellphoneNumber, email);

            _signUpViewModel.SignUpCommand.Execute();

            _signUpService.SignUpInformation.Should().BeEquivalentTo(expectedSignUpInformation);
        }

        [Test]
        public void WhenSigningUpShouldNavigateToTokenPage()
        {
            _signUpViewModel.SignUpCommand.Execute();
            _navigationService.Name.Should().Be(nameof(TokenPage));
        }

        [Test]
        public void WhenSingingUpWithAnExistingCPFShouldDisplayAlertWithErrorMessage()
        {
            const string message = "CPF já cadastrado.";
            const string cancelButton = "OK";

            _signUpViewModel.FullName = fullName;
            _signUpViewModel.CPF = cpf;
            _signUpViewModel.CellphoneNumber = cellphoneNumber;
            _signUpViewModel.Email = email;

            _signUpService.ShouldValidateExistingCpf(true);

            _signUpViewModel.SignUpCommand.Execute();

            _pageDialogService.Title.Should().Be(_signUpViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }
    }
}
