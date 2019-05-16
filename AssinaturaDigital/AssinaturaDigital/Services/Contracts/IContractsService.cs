using AssinaturaDigital.Models;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Contracts
{
    public interface IContractsService
    {
        Task<IList<ContractData>> GetContracts(int idUser);
        Task<bool> SignContract(int id, int idUser, MediaFile photo);
    }
}