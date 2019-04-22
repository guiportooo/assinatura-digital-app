using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

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
        public void WhenCreatingViewModelShouldPopulateTitle() => _infoSelfieViewModel.Title.Should().Be("Info Selfie");

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
            _infoSelfieViewModel.TakeSelfieCommand.Execute();
            _navigationService.Name.Should().Be(nameof(SelfiePage));
        }
    }
}
