using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using System;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class InfoSigningContractViewModelTests
    {
        private readonly int _idUser = 1;
        private InfoSigningContractViewModel _infoSigningContractViewModel;
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private ContractsServiceFake _contractsService;
        private Mock<IPreferences> _preferencesMock;

        [SetUp]
        public void Setup()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _contractsService = new ContractsServiceFake();
            _preferencesMock = new Mock<IPreferences>();

            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(_idUser);

            _infoSigningContractViewModel = new InfoSigningContractViewModel(
                _navigationService,
                _pageDialogService,
                _contractsService,
                _preferencesMock.Object);
        }

        [Test]
        public void WhenNavigatingToPageShouldGoBackToSelfiePageIfParametersAreInvalid()
        {
            _infoSigningContractViewModel.OnNavigatingTo(null);
            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void WhenNavigatingToPageShouldSetSignContractAndSignedMessageIfContractWasApproved()
        {
            const int idContract = 1;
            var contract = new ContractData(idContract, "", "", "", false);

            var parameters = new NavigationParameters
            {
                { AppConstants.Selfie, new MediaFile("Selfie", null) },
                { AppConstants.Contract, contract }
            };

            _infoSigningContractViewModel.OnNavigatingTo(parameters);

            _infoSigningContractViewModel.Contract.IsSigned.Should().BeTrue();
            _infoSigningContractViewModel.Title.Should().Be("Contrato assinado!");
            _infoSigningContractViewModel.Signed.Should().BeTrue();
            _infoSigningContractViewModel.Message.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenNavigatingToPageShouldSetNotSignedMessageIfContractWasNotApproved()
        {
            const int idContract = 1;
            var contract = new ContractData(idContract, "", "", "", false);

            var parameters = new NavigationParameters
            {
                { AppConstants.Selfie, new MediaFile("Selfie", null) },
                { AppConstants.Contract, contract }
            };

            _contractsService.ShouldNotSignContract();

            _infoSigningContractViewModel.OnNavigatingTo(parameters);

            _infoSigningContractViewModel.Title.Should().Be("Alerta");
            _infoSigningContractViewModel.Signed.Should().BeFalse();
        }
    }
}
