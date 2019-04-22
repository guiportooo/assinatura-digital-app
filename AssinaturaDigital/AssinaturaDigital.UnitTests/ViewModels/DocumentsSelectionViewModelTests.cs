using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class DocumentsSelectionViewModelTests
    {
        private DocumentsSelectionViewModel _documentsViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _documentsViewModel = new DocumentsSelectionViewModel(_navigationService, _pageDialogService);
        }

        [Test]
        public void WhenChooseRGShouldNavigateToRGPage()
        {
            _documentsViewModel.ChooseRGPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
        }

        [Test]
        public void WhenChooseCNHShouldNavigateToCNHPage()
        {
            _documentsViewModel.ChooseCNHPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(DocumentPage));
        }
    }
}
