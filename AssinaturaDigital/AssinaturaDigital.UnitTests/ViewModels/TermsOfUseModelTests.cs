using AssinaturaDigital.Events;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
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
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private TermsOfUseServiceFake _userTermsServiceFake;
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _userTermsServiceFake = new TermsOfUseServiceFake();
            _eventAggregator = new EventAggregator();

            _userTermsViewModel = new TermsOfUseViewModel(_navigationService,
                _pageDialogService,
                _userTermsServiceFake,
                _eventAggregator);
        }

        [Test]
        public async Task WhenNavigateToPageShouldGetTermsOfUse()
        {
            var expectedTerm = await _userTermsServiceFake.GetTermsUse();
            _userTermsViewModel.OnNavigatingTo(new NavigationParameters());
            _userTermsViewModel.TermsOfUse.Should().Be(expectedTerm);
        }

        [Test]
        public void WhenScrollingToEndOfPageShouldMarkTermsAsRead()
        {
            _userTermsViewModel.OnNavigatedTo(new NavigationParameters());
            _eventAggregator.GetEvent<ScrolledToBottomEvent>().Publish();
            _userTermsViewModel.ReadTerms.Should().BeTrue();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldValidateIfTermsWereAccepted(bool acceptedTerms)
        {
            _userTermsViewModel.AcceptedTerms = acceptedTerms;
            _userTermsViewModel.AcceptTermsCommand.CanExecute().Should().Be(acceptedTerms);
        }

        [Test]
        public void WhenAcceptingTermsShouldNavigateToDocumentsSelecitonPage()
        {
            _userTermsViewModel.AcceptedTerms = true;
            _userTermsViewModel.AcceptTermsCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentsSelectionPage));
        }
    }
}
