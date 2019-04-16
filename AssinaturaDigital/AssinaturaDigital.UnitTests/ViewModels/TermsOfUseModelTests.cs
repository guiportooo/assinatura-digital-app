using AssinaturaDigital.Events;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using FluentAssertions;
using NUnit.Framework;
using Prism.Events;
using Prism.Navigation;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class TermsOfUseModelTests
    {
        private TermsOfUseViewModel _userTermsViewModel;
        private TermsOfUseServiceFake _userTermsServiceFake;
        private NavigationServiceMock _navigationService;
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _userTermsServiceFake = new TermsOfUseServiceFake();
            _eventAggregator = new EventAggregator();

            _userTermsViewModel = new TermsOfUseViewModel(
                _userTermsServiceFake,
                _eventAggregator,
                _navigationService);
        }

        [Test]
        public async Task WhenNavigateToPageShouldGetTermsOfUse()
        {
            var expectedTerm = await _userTermsServiceFake.GetTermsUse();
            _userTermsViewModel.OnNavigatingTo(new NavigationParameters());
            _userTermsViewModel.TermsOfUse.Should().Be(expectedTerm);
        }

        [Test]
        public void WhenScrollToEndOfPageButtonReadTermsShouldBeTrue()
        {
            _userTermsViewModel.OnNavigatedTo(new NavigationParameters());
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Publish();
            _userTermsViewModel.ReadTerms.Should().BeTrue();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnlyNavigateToNextPageWhenAcceptedTermsShouldBeTrue(bool acceptedTerms)
        {
            _userTermsViewModel.AcceptedTerms = acceptedTerms;
            _userTermsViewModel.AcceptTermsCommand.CanExecute().Should().Be(acceptedTerms);
        }
    }
}
