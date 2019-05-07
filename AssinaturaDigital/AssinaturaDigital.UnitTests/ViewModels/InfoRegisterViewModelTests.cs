using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class InfoRegisterViewModelTests
    {
        private readonly int _idUser = 1;
        private InfoRegisterViewModel _infoRegisterViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private SelfiesServiceFake _selfiesService;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _selfiesService = new SelfiesServiceFake();
            _preferencesMock = new Mock<IPreferences>();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _infoRegisterViewModel = new InfoRegisterViewModel(
                _navigationService,
                _pageDialogService,
                _selfiesService,
                _preferencesMock.Object);
        }

        [Test]
        public void WhenNavigatingToPageShouldGoBackToSelfiePageIfParametersAreInvalid()
        {
            _infoRegisterViewModel.OnNavigatingTo(null);
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void WhenNavigatingToPageShouldSetApprovedMessageIfSelfieIsValid()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.Selfie, new MediaFile("Selfie", null) }
            };

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Quase pronto para assinar!");
            _infoRegisterViewModel.Message.Should().Be("Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.");
        }

        [Test]
        public void WhenNavigatingToPageShouldSetNotApprovedMessageIfSelfieIsNotValid()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.Selfie, new MediaFile("Selfie", null) }
            };

            _selfiesService.ShouldNotBeValid();

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Imagem inválida!");
            _infoRegisterViewModel.Message.Should().Be("Você será redirecionado para uma nova selfie.");
        }

        [Test]
        public void WhenNavigatingToHomeShouldNavigateToMainPage()
        {
            _infoRegisterViewModel.NavigateToHomeCommand.Execute();
            _navigationService.Name.Should().Be(nameof(HomePage));
        }

        [Test]
        public void WhenLogginOutShouldNavigateToMainPage()
        {
            _infoRegisterViewModel.LogoutCommand.Execute();
            _navigationService.Name.Should().Be(nameof(MainPage));
        }
    }
}
