using AssinaturaDigital.ViewModels;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests
{
    public class MainViewModelTests
    {
        [Test]
        public void ShouldCreateViewModelWithWelcomeMessage()
        {
            const string welcomeMessage = "Welcome to Xamarin.Forms with Prism!";
            var mainViewModel = new MainViewModel();
            mainViewModel.WelcomeMessage.Should().Be(welcomeMessage);
        }
    }
}