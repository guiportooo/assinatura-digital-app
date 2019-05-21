using AssinaturaDigital.Models;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Prism.Navigation;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class RGOrientationViewModelTests
    {
        private RGOrientationViewModel _rgOrientationViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _rgOrientationViewModel = new RGOrientationViewModel(_navigationService, _pageDialogService);
        }

        [Test]
        public void WhenCloseModalShouldNavigateToRGPage()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.DocumentType, DocumentType.RG }
            };

            _rgOrientationViewModel.CloseModalCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }
    }
}
