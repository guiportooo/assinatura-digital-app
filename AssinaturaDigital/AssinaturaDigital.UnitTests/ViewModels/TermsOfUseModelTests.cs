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
        public void WhenNavigatingToPageShouldConfigureSteps()
        {
            var expectedCurrentStep = 3;
            var expectedCountListSteps = 5;
            _userTermsViewModel.CurrentStep.Should().Be(expectedCurrentStep);
            _userTermsViewModel.StepsList.Count.Should().Be(expectedCountListSteps);
            _userTermsViewModel.StepsList[0].Done.Should().BeTrue();
            _userTermsViewModel.StepsList[1].Done.Should().BeTrue();
            _userTermsViewModel.StepsList[2].Done.Should().BeTrue();
            _userTermsViewModel.StepsList[3].Done.Should().BeFalse();
            _userTermsViewModel.StepsList[4].Done.Should().BeFalse();
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

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void ShouldNotBeAbleToGoFowardIfPageIsBusy(bool isBusy, bool canGoFoward)
        {
            _userTermsViewModel.AcceptedTerms = true;

            _userTermsViewModel.IsBusy = isBusy;
            _userTermsViewModel.GoFowardCommand.CanExecute().Should().Be(canGoFoward);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void ShouldValidateIfTermsWereAccepted(bool acceptedTerms, bool canGoFoward)
        {
            _userTermsViewModel.IsBusy = false;

            _userTermsViewModel.AcceptedTerms = acceptedTerms;
            _userTermsViewModel.GoFowardCommand.CanExecute().Should().Be(canGoFoward);
        }

        [Test]
        public void WhenAcceptingTermsShouldNavigateToDocumentsSelectionPage()
        {
            _userTermsViewModel.AcceptedTerms = true;
            _userTermsViewModel.GoFowardCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentsSelectionPage));
        }

        [Test]
        public void WhenFailingToNavigateToDocumentsSelectionPageShouldDisplayAlertWithErrorMessage()
        {
            const string message = "Falha ao aceitar termos de uso.";
            const string cancelButton = "OK";

            _navigationService.ShouldFail(true);

            _userTermsViewModel.GoFowardCommand.Execute();

            _pageDialogService.Title.Should().Be(_userTermsViewModel.Title);
            _pageDialogService.Message.Should().Be(message);
            _pageDialogService.CancelButton.Should().Be(cancelButton);
        }
    }
}
