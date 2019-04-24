using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Prism.Navigation;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class InfoRegisterViewModelTests
    {
        private InfoRegisterViewModel _infoRegisterViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _infoRegisterViewModel = new InfoRegisterViewModel(
                _navigationService,
                _pageDialogService);
        }

        [Test]
        public void WhenNavigatingToPageShouldGoBackToSelfiePageIfParametersAreInvalid()
        {
            _infoRegisterViewModel.OnNavigatingTo(null);
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void WhenNavigatingToPageShouldSetApprovedMessage()
        {
            const bool approved = true;

            var parameters = new NavigationParameters
            {
                { AppConstants.ApprovedSelfie, approved }
            };

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Quase pronto para assinar!");
            _infoRegisterViewModel.Message.Should().Be("Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.");
        }

        [Test]
        public void WhenNavigatingToPageShouldSetNotApprovedMessage()
        {
            const bool approved = false;

            var parameters = new NavigationParameters
            {
                { AppConstants.ApprovedSelfie, approved }
            };

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Imagem inválida!");
            _infoRegisterViewModel.Message.Should().Be("Você será redirecionado para uma nova selfie.");
        }

        [Test]
        public void WhenNavigatingToHomeShouldNavigateToMainPage()
        {
            _infoRegisterViewModel.NavigateToHomeCommand.Execute();
            _navigationService.Name.Should().Be(nameof(MainPage));
        }

        [Test]
        public void WhenLogginOutShouldNavigateToMainPage()
        {
            _infoRegisterViewModel.LogoutCommand.Execute();
            _navigationService.Name.Should().Be(nameof(MainPage));
        }
    }
}
