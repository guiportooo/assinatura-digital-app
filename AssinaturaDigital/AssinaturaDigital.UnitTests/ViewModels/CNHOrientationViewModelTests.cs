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
    public class CNHOrientationViewModelTests
    {
        private CNHOrientationViewModel _cnhOrientationViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _cnhOrientationViewModel = new CNHOrientationViewModel(_navigationService, _pageDialogService);
        }

        [Test]
        public void WhenCloseModalShouldNavigateToCNHPage()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.DocumentType, DocumentType.CNH }
            };

            _cnhOrientationViewModel.CloseModalCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
            _navigationService.Parameters.Should().BeEquivalentTo(expectedParameters);
        }
    }
}
