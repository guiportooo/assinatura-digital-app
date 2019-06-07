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
        private ValidationsServiceFake _validationsServiceFake;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _validationsServiceFake = new ValidationsServiceFake();
            _preferencesMock = new Mock<IPreferences>();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _infoRegisterViewModel = new InfoRegisterViewModel(
                _navigationService,
                _pageDialogService,
                _validationsServiceFake,
                _preferencesMock.Object);
        }

        [Test]
        public void WhenNavigatingToPageShouldGoBackToVideoPageIfParametersAreInvalid()
        {
            _infoRegisterViewModel.OnNavigatingTo(null);
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void WhenNavigatingToPageShouldSetApprovedMessageIfVideoIsValid()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.Video, new MediaFile("Video", null) }
            };

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Quase pronto para assinar!");
            _infoRegisterViewModel.Message.Should().Be("Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.");
        }

        [Test]
        public void WhenNavigatingToPageShouldSetNotApprovedMessageIfVideoIsNotValid()
        {
            var parameters = new NavigationParameters
            {
                { AppConstants.Video, new MediaFile("Video", null) }
            };

            _validationsServiceFake.ShouldNotBeValid();

            _infoRegisterViewModel.OnNavigatingTo(parameters);

            _infoRegisterViewModel.Title.Should().Be("Cadastro não finalizado!");
            _infoRegisterViewModel.Message.Should().Be("Seu cadastro não pôde ser finalizado por divergências de verificação nas imagens e vídeo enviados. O problema pode ter ocorrido:\n\n- Nas imagens do documento (RG ou CNH)\n\n- No vídeo\n\nPor favor, repita o processo de envio de imagens e vídeo para que uma nova validação seja realizada.");
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
