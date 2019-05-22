using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Contracts;
using AssinaturaDigital.Services.Manifest;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AssinaturaDigital.Services.Fakes
{
    public class ContractsServiceFake : IContractsService
    {
        private bool _shouldNotSignContract;

        public IList<ContractData> Contracts;
        public int Id;
        public int IdUser; 

        const string contractData =
        @"                 
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.                 
        Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.                 
        Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.  

        Lorem ipsum dolor sit amet, consectetur adipiscing elit.                 
        Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.                 
        Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.                  

        Maecenas aliquam massa id malesuada porttitor.                 
        Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.                 
        Phasellus sed neque sem. Nulla facilisi.                  
        
        Maecenas aliquam massa id malesuada porttitor.                 
        Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.                 
        Phasellus sed neque sem. Nulla facilisi.              
        ";


        public void ShouldNotSignContract() => _shouldNotSignContract = true;

        public ContractsServiceFake() => Contracts = new List<ContractData>
            {
                new ContractData(1, "Identificação 01", "000.000.000-00", contractData, false),
                new ContractData(2, "Identificação 02", "000.000.000-00", contractData, true),
                new ContractData(3, "Identificação 03", "000.000.000-00", contractData, false),
                new ContractData(4, "Identificação 04", "000.000.000-00", contractData, false),
                new ContractData(5, "Identificação 05", "000.000.000-00", contractData, false)
            };

        public Task<IList<ContractData>> GetContracts(int idUser)
        {
            IdUser = idUser;
            return Task.FromResult(Contracts);
        }

        public ContractData GetContract(string identification)
            => Contracts.FirstOrDefault(x => x.Identification == identification);

        public Task<bool> SignContract(int id, int idUser, MediaFile photo, ManifestInfos manifestInfos)
        {
            try
            {
                Id = id;
                IdUser = idUser;

                if (_shouldNotSignContract)
                    return Task.FromResult(false);

                Contracts.FirstOrDefault(x => x.Id == id).Sign();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
