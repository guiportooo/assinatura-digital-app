using AssinaturaDigital.Models; using AssinaturaDigital.Services.Interfaces; using System; using System.Collections.Generic; using System.Threading.Tasks; using System.Linq;  namespace AssinaturaDigital.Services.Fakes {     public class ContractService : IContractService     {         public List<ContractData> Contracts = new List<ContractData>();         public int IdUser;          const string contractData =             @"                 Lorem ipsum dolor sit amet, consectetur adipiscing elit.                 Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.                 Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.                  Lorem ipsum dolor sit amet, consectetur adipiscing elit.                 Nulla convallis ut nunc at ornare. Quisque sed ultrices urna, at pretium turpis.                 Suspendisse ultricies et ex a blandit. Phasellus facilisis sem eget nunc dictum congue.                  Maecenas aliquam massa id malesuada porttitor.                 Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.                 Phasellus sed neque sem. Nulla facilisi.                  Maecenas aliquam massa id malesuada porttitor.                 Nunc odio felis, dignissim non sollicitudin rhoncus, commodo suscipit leo.                 Phasellus sed neque sem. Nulla facilisi.              ";          public ContractService()         {             Contracts.Add(new ContractData("Identificação 01", "000.000.000-00", contractData, false));             Contracts.Add(new ContractData("Identificação 02", "000.000.000-00", contractData, true));             Contracts.Add(new ContractData("Identificação 03", "000.000.000-00", contractData, false));             Contracts.Add(new ContractData("Identificação 04", "000.000.000-00", contractData, false));             Contracts.Add(new ContractData("Identificação 05", "000.000.000-00", contractData, false));         }          public async Task<List<ContractData>> GetContracts(int idUser)
        {
            IdUser = idUser;             return Contracts;
        }
                    public Task<bool> SignContract(string identification, int idUser)         {             try             {                 Contracts.FirstOrDefault(x => x.Identification == identification).SignContract();                 return Task.FromResult(true);             }             catch (Exception e)             {                 return Task.FromResult(false);;             }         }          public ContractData GetContract(string identification)
        {
            return Contracts.FirstOrDefault(x => x.Identification == identification);
        }     } }