using AssinaturaDigital.Models; using System; using System.Collections.Generic; using System.Threading.Tasks;  namespace AssinaturaDigital.Services.Interfaces {     public interface IContractService     {         Task<List<ContractData>> GetContracts(int idUser);         Task<bool> SignContract(string identification, int idUser);         ContractData GetContract(string identification);     } }  