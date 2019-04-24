using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class MainViewModelTests
    {
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private MainViewModel _mainViewModel;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _mainViewModel = new MainViewModel(_navigationService, _pageDialogService);
        }

        [Test]
        public void OnOpenSignUpShouldNavigateToSignUpPage()
        {
            _mainViewModel.OpenSignUpCommand.Execute();
            _navigationService.Name.Should().Be(nameof(SignUpPage));
        }
    }
}