using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests
{
    public class MainViewModelTests
    {
        private NavigationServiceMock _navigation;
        private MainViewModel _mainViewModel;

        [SetUp]
        public void Setup()
        {
            _navigation = new NavigationServiceMock();
            _mainViewModel = new MainViewModel(_navigation);
        }

        [Test]
        public void ShouldNavigateToSignUpPage()
        {
            _mainViewModel.OpenSignUpCommand.Execute();
            _navigation.Name.Should().Be(nameof(SignUpPage));
        }
    }
}