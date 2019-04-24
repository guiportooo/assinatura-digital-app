using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class DocumentsSelectionViewModelTests
    {
        private DocumentsSelectionViewModel _documentsSelectionViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _documentsSelectionViewModel = new DocumentsSelectionViewModel(_navigationService, _pageDialogService);
        }

        [Test]
        public void WhenCreatingViewModelShouldPopulateTitleAndRemoveFowardNavigation()
        {
            _documentsSelectionViewModel.Title.Should().Be("Documento");
            _documentsSelectionViewModel.HasFowardNavigation.Should().BeFalse();
        }

        [Test]
        public void WhenNavigatingToPageShouldConfigureSteps()
        {
            var expectedCurrentStep = 4;
            var expectedCountListSteps = 5;
            _documentsSelectionViewModel.CurrentStep.Should().Be(expectedCurrentStep);
            _documentsSelectionViewModel.StepsList.Count.Should().Be(expectedCountListSteps);
            _documentsSelectionViewModel.StepsList[0].Done.Should().BeTrue();
            _documentsSelectionViewModel.StepsList[1].Done.Should().BeTrue();
            _documentsSelectionViewModel.StepsList[2].Done.Should().BeTrue();
            _documentsSelectionViewModel.StepsList[3].Done.Should().BeTrue();
            _documentsSelectionViewModel.StepsList[4].Done.Should().BeFalse();
        }

        [Test]
        public void WhenChooseRGShouldNavigateToRGPage()
        {
            _documentsSelectionViewModel.ChooseRGPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
        }

        [Test]
        public void WhenChooseCNHShouldNavigateToCNHPage()
        {
            _documentsSelectionViewModel.ChooseCNHPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
        }
    }
}
