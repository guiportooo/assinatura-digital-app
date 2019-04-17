using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using NUnit.Framework;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class DocumentsViewModelTests
    {
        private DocumentsViewModel _documentsViewModel;
        private NavigationServiceMock _navigationService;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _documentsViewModel = new DocumentsViewModel(_navigationService);
        }

        [Test]
        public void WhenChooseRGShouldNavigateToRGPage()
        {
            _documentsViewModel.ChooseRGPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(RGPage));
        }

        [Test]
        public void WhenChooseCNHShouldNavigateToCNHPage()
        {
            _documentsViewModel.ChooseCNHPictureCommand.Execute();
            _navigationService.Name.Should().Be(nameof(CNHPage));
        }
    }
}
