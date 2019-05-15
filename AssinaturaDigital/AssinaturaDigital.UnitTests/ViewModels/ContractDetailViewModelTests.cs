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
using System;
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
        private ContractService _contractService;


        private ContractDetailViewModel _vm;

        [SetUp]
        public void SetUp()
        {
            _navigationService = new NavigationServiceMock();
            _pageDialogService = new PageDialogServiceMock();
            _contractService = new ContractService();
            _preferencesMock = new Mock<IPreferences>();
            _eventAggregator = new EventAggregator();

            _vm = new ContractDetailViewModel(_navigationService, _pageDialogService, _contractService, _eventAggregator, _preferencesMock.Object);
        }

        [Test]
        public void OnNavigatedShouldGetCorrectContractFromService()
        {
            var contractIdentifier = "Identificação 01";
            var expectedContract = _contractService.Contracts.FirstOrDefault(x => x.Identification == contractIdentifier);

            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.ContractIdentification, contractIdentifier);
            _vm.OnNavigatedTo(parameters);
            
            _vm.Contract.Should().BeEquivalentTo(expectedContract);
        }

        [Test]
        public void OnNavigatedIfDontHasTheContractIdentifierShouldBackToContractListPage()
        {
            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.IdUser, 0);

            _vm.OnNavigatedTo(parameters);

            _navigationService.Name.Should().Be(nameof(ContractListPage));
            _navigationService.Parameters.Should().BeEquivalentTo(parameters);
        }

        [Test]
        public void GoFowardShouldNavigateIfAgreeContractIsTrue()
        {
            var contractIdentifier = "Identificação 01";
            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.ContractIdentification, contractIdentifier);
            _vm.OnNavigatedTo(parameters);

            _vm.AgreeContract = true;
            _vm.GoFowardCommand.Execute();

            var parameters2 = new NavigationParameters();
            parameters2.Add(AppConstants.SigningContract, true);
            parameters2.Add(AppConstants.Registered, true);
            parameters2.Add(AppConstants.Contract, _vm.Contract);

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

            var parameters = new NavigationParameters();
            parameters.Add(AppConstants.IdUser, 0);

            _navigationService.Name.Should().Be(nameof(ContractListPage));
            _navigationService.Parameters.Should().BeEquivalentTo(parameters);

        }

    }
}
