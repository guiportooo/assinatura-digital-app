using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.UnitTests.Mocks;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Prism.Events;
using Prism.Navigation;
using System.Linq;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.UnitTests.ViewModels
{
    public class ContractDetailViewModelTests
    {
        private NavigationServiceMock _navigationService;
        private PageDialogServiceMock _pageDialogService;
        private Mock<IPreferences> _preferencesMock;
        private IEventAggregator _eventAggregator;
        private ContractsServiceFake _contractService;


        private ContractDetailViewModel _vm;

        [SetUp]
        public void SetUp()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _contractService = new ContractsServiceFake();
            _preferencesMock = new Mock<IPreferences>();
            _eventAggregator = new EventAggregator();

            _vm = new ContractDetailViewModel(_navigationService, _pageDialogService, _contractService, _eventAggregator, _preferencesMock.Object);
        }

        [Test]
        public void AfterNavigatedShouldNotHaveFowardNavigationIfContractIsSigned()
        {
            var contractIdentifier = "Identificação 01";
            var contract = _contractService.Contracts.FirstOrDefault(x => x.Identification == contractIdentifier);
            contract.Sign();

            var parameters = new NavigationParameters
            {
                { AppConstants.Contract, contract }
            };

            _vm.OnNavigatedTo(parameters);

            _vm.HasFowardNavigation.Should().BeFalse();
        }

        [Test]
        public void OnNavigatedShouldGetCorrectContractFromService()
        {
            var contractIdentifier = "Identificação 01";
            var expectedContract = _contractService.Contracts.FirstOrDefault(x => x.Identification == contractIdentifier);
            var parameters = new NavigationParameters
            {
                { AppConstants.Contract, expectedContract }
            };
            _vm.OnNavigatedTo(parameters);

            _vm.Contract.Should().BeEquivalentTo(expectedContract);
        }

        [Test]
        public void OnNavigatedIfDontHasTheContractShouldBackToContractListPage()
        {
            var parameters = new NavigationParameters();

            _vm.OnNavigatedTo(parameters);

            _navigationService.WentBack.Should().BeTrue();
        }

        [Test]
        public void GoFowardShouldNavigateIfAgreeContractIsTrue()
        {
            var contractIdentifier = "Identificação 01";
            var contract = _contractService.Contracts.FirstOrDefault(x => x.Identification == contractIdentifier);
            var parameters = new NavigationParameters
            {
                { AppConstants.Contract, contract }
            };
            _vm.OnNavigatedTo(parameters);

            _vm.AgreeContract = true;
            _vm.GoFowardCommand.Execute();

            var parameters2 = new NavigationParameters
            {
                { AppConstants.SigningContract, true },
                { AppConstants.Registered, true },
                { AppConstants.Contract, _vm.Contract }
            };

            _navigationService.Name.Should().Be(nameof(InfoSelfiePage));
            _navigationService.Parameters.Should().BeEquivalentTo(parameters2);
        }

        [Test]
        public void GoFowardShouldDisplayMessageIfAgreeContractIsFalse()
        {
            _vm.AgreeContract = false;
            _vm.GoFowardCommand.Execute();
            _pageDialogService.Message.Should().Be("Necessário aceitar o termo de uso para prosseguir.");
        }

        [Test]
        public void GoHomeCommandShouldNavigateToHomePage()
        {
            _vm.GoHomeCommand.Execute();
            _navigationService.Name.Should().Be(nameof(HomePage));
        }

        [Test]
        public void GoContractListCommandShouldNavigateToContractListPage()
        {
            _vm.GoContractListCommand.Execute();
            _navigationService.Name.Should().Be(nameof(ContractListPage));
        }
    }
}
