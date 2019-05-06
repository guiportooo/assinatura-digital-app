using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SignUpViewModelTests
    {
        const string fullName = "Test User";
        const string cpf = "222.222.222-22";
        const string cellPhoneNumber = "987654321";
        const string email = "test.user@appsign.com";

        private SignUpViewModel _signUpViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private SignUpServiceFake _signUpService;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _signUpService = new SignUpServiceFake();
            _signUpService.ShouldDelay(false);
            _preferencesMock = new Mock<IPreferences>();
            _signUpViewModel = new SignUpViewModel(_navigationService, _pageDialogService, _signUpService, _preferencesMock.Object);
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
            _signUpViewModel.IsBusy = pageIsBusy;
            _signUpViewModel.GoFowardCommand.CanExecute().Should().Be(canSignUp);
        }

        [Test]
        public void WhenSigningUpShouldSendInformationToTheService()
        {
            _signUpViewModel.FullName = fullName;
            _signUpViewModel.CPF = cpf;
            _signUpViewModel.CellPhoneNumber = cellPhoneNumber;
            _signUpViewModel.Email = email;

            var expectedSignUpInformation = new SignUpInformation(fullName, cpf, cellPhoneNumber, email);

            _signUpViewModel.GoFowardCommand.Execute();

            _signUpService.SignUpInformation.Should().BeEquivalentTo(expectedSignUpInformation);
        }

        [Test]
        public void WhenSigningUpShouldNavigateToTokenPage()
        {
            _signUpViewModel.GoFowardCommand.Execute();
            _navigationService.Name.Should().Be(nameof(TokenPage));
        }

        [Test]
        public void WhenSingingUpWithAnExistingCPFShouldDisplayAlertWithErrorMessage()
        {
            const string message = "CPF já cadastrado.";
            const string cancelButton = "OK";

            _signUpViewModel.FullName = fullName;
            _signUpViewModel.CPF = cpf;
            _signUpViewModel.CellPhoneNumber = cellPhoneNumber;
            _signUpViewModel.Email = email;

            _signUpService.ShouldValidateExistingCpf(true);

            _signUpViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_signUpViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }
    }
}
