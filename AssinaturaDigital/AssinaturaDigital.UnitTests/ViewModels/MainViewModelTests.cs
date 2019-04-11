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
        private MainViewModel _mainViewModel;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _mainViewModel = new MainViewModel(_navigationService);
        }

        [Test]
        public void OnOpenSignUpShouldNavigateToSignUpPage()
        {
            _mainViewModel.OpenSignUpCommand.Execute();
            _navigationService.Name.Should().Be(nameof(SignUpPage));
        }
    }
}