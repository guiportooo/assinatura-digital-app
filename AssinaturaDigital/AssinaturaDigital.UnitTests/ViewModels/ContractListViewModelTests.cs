using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using System.Linq;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class ContractListViewModelTests
    {
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private Mock<IPreferences> _preferencesMock;
        private ContractService _contractService;

        private ContractListViewModel _vm;

        [SetUp]
        public void SetUp()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _contractService = new ContractService();
            _preferencesMock = new Mock<IPreferences>();

            _vm = new ContractListViewModel(_navigationService, _pageDialogService, _contractService, _preferencesMock.Object);
        }

        [Test]
        public void ShouldLoadContractsListWhenNavigateTo()
        {
            _vm.OnNavigatedTo(null);
            _vm.Contracts.Should().NotBeEmpty();
            _vm.Contracts.Should().BeEquivalentTo(_contractService.Contracts);
        }

        [Test]
        public void ShouldPassIdUserToContractService()
        {
            _preferencesMock.Setup(x => x.Get(AppConstants.IdUser, 0)).Returns(1);
            _vm.OnNavigatedTo(null);
            _contractService.IdUser.Should().Be(_preferencesMock.Object.Get(AppConstants.IdUser, 0));
        }

        [Test]
        public void ShouldShowOnlyUnSignedContracts()
        {
            var expectedContracts = _contractService
                .Contracts
                .Where(x => x.IsSigned == false);

            _vm.OnNavigatedTo(null);

            _vm.FiltreContracts(false);

            _vm.Contracts.Should().BeEquivalentTo(expectedContracts);
        }

        [Test]
        public void ShouldShowUnSignedAndSignedContracts()
        {
            var expectedContracts = _contractService
                .Contracts;

            _vm.OnNavigatedTo(null);

            _vm.FiltreContracts(true);

            _vm.Contracts.Should().BeEquivalentTo(expectedContracts);
        }

        [Test]
        public void ReloadContractsShouldResetContractsIfParameterIsNullOrEmpty()
        {
            _vm.OnNavigatedTo(null);

            _vm.FiltreContracts(false);

            _vm.ReloadContractList("");

            _vm.Contracts.Should().BeEquivalentTo(_contractService.Contracts);

        }

        [Test]
        public void ReloadContractsShouldNotResetListIfTextParameterHasValue()
        {

            var expectedContracts = _contractService
               .Contracts
               .Where(x => x.IsSigned == false);

            _vm.OnNavigatedTo(null);

            _vm.FiltreContracts(false);
           
            _vm.ReloadContractList("aaaaa");

            _vm.Contracts.Should().BeEquivalentTo(expectedContracts);

        }

        [Test]
        public void SearchContractShouldReturnCorrectContract()
        {
            var expectedContracts = _contractService
               .Contracts
               .Where(x => x.Identification == "Identificação 03");

            _vm.OnNavigatedTo(null);
            _vm.SearchContractCommand.Execute("Identificação 03");

            _vm.Contracts.Should().BeEquivalentTo(expectedContracts);

        }

        [Test]
        public void SearchContractShouldNotFiltreListIfNotFound()
        {
            _vm.OnNavigatedTo(null);
            _vm.SearchContractCommand.Execute("aaaaaaaaa");

            _vm.Contracts.Should().BeEquivalentTo(_contractService.Contracts);

        }

        [Test]
        public void OpenContractDetailsShouldSendContractIdentificationAndNavigateToContractDetailsPage()
        {
            var expectedParameters = new NavigationParameters
            {
                { AppConstants.ContractIdentification, "Identificação 03" }
            };

            _vm.OnNavigatedTo(null);
            _vm.OpenContractDetailsCommand.Execute("Identificação 03");
            
            _navigationService.Name.Should().Be(nameof(ContractDetailPage));
            _navigationService
            .Parameters
            .Should()
            .Contain(expectedParameters);

        }
    }
}
