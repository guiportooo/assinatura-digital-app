using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Prism.Navigation;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class InfoSelfieViewModelTests
    {
        private InfoSelfieViewModel _infoSelfieViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _infoSelfieViewModel = new InfoSelfieViewModel(
                _navigationService,
                _pageDialogService);
        }

        [Test]
        public void WhenNavigatingToPageShouldConfigureSteps()
        {
            var expectedCurrentStep = 5;
            var expectedCountListSteps = 5;
            _infoSelfieViewModel.CurrentStep.Should().Be(expectedCurrentStep);
            _infoSelfieViewModel.StepsList.Count.Should().Be(expectedCountListSteps);
            _infoSelfieViewModel.StepsList[0].Done.Should().BeTrue();
            _infoSelfieViewModel.StepsList[1].Done.Should().BeTrue();
            _infoSelfieViewModel.StepsList[2].Done.Should().BeTrue();
            _infoSelfieViewModel.StepsList[3].Done.Should().BeTrue();
            _infoSelfieViewModel.StepsList[4].Done.Should().BeTrue();
        }

        [Test]
        public void WhenExecuteInfoSelfieCommandShouldGetInfosSelfie()
        {
            var expectedInfos = "Para tornar a assinatura do contrato mais segura e possibilitar maior praticidade no processo, transformaremos sua foto em um algoritmo biométrico para identificação.";
            _infoSelfieViewModel.ShowInfoCommand.Execute();
            _pageDialogService.Message.Should().Be(expectedInfos);
        }

        [Test]
        public void SelfieCommandShouldNavigateToSelfiePage()
        {
            _infoSelfieViewModel.GoFowardCommand.Execute();
            _navigationService.Name.Should().Be(nameof(SelfiePage));
        }

        [Test]
        public void WhenFailingToNavigateToSelfiePageShouldDisplayAlertWithErrorMessage()
        {
            const string message = "Falha ao avançar para tela de selfie.";
            const string cancelButton = "OK";

            _navigationService.ShouldFail(true);

            _infoSelfieViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_infoSelfieViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }

        [Test]
        public void OnNavigatedToSigningContractShouldSetCorrectTitleAndSubTitle()
        {
            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.SigningContract, true);

            _infoSelfieViewModel.OnNavigatingTo(parameters);
            _infoSelfieViewModel.Title.Should().Be("Chegou a hora de assinar");
            _infoSelfieViewModel.SubTitle.Should().Be("Sua foto assinará o contrato automaticamente");
        }

        [Test]
        public void OnNavigatedToSignInUserShouldSetCorrectTitleAndSubTitle()
        {
            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.SigningContract, false);

            _infoSelfieViewModel.OnNavigatingTo(parameters);
            _infoSelfieViewModel.Title.Should().Be("Estamos quase terminando");
            _infoSelfieViewModel.SubTitle.Should().Be("Agora vamos tirar uma selfie");
        }
    }
}
