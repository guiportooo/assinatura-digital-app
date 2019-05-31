using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class SelfieOrientationViewModelTests
    {
        private SelfieOrientationViewModel _selfieOrientationViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _selfieOrientationViewModel = new SelfieOrientationViewModel(
                _navigationService,
                _pageDialogService);
        }

        [Test]
        public void WhenGoingFowardShouldNavigateToSelfiePage()
        {
            _selfieOrientationViewModel.GoFowardCommand.Execute();
            _navigationService.Name.Should().Be(nameof(SelfiePage));
        }
    }
}
