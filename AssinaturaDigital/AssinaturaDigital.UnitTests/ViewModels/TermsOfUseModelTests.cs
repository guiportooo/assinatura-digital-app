using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Prism.Navigation;
using System.Threading.Tasks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Services.Fakes;
using Prism.Services;
using AssinaturaDigital.UnitTests.Mocks;
using Prism.Events;
using AssinaturaDigital.Events;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class TermsOfUseModelTests
    {
        private TermsOfUseViewModel _userTermsViewModel;
        private TermsOfUseServiceFake _userTermsServiceFake;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void Setup()
        {
            _pageDialogService = new PageDialogServiceMock();
            _userTermsServiceFake = new TermsOfUseServiceFake();
            _navigationService = new NavigationServiceMock();
            _eventAggregator = new EventAggregator();

            _userTermsViewModel = new TermsOfUseViewModel(
                _userTermsServiceFake,
                _pageDialogService,
                _eventAggregator,
                _navigationService);
        }

        [Test]
        public async Task WhenNavigateToPageShouldGetTermsOfUse()
        {
            var expectedTerm = await _userTermsServiceFake.GetTermsUse();
            _userTermsViewModel.OnNavigatingTo(new NavigationParameters());

            _userTermsViewModel.TermsUse.Should().Be(expectedTerm);
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
