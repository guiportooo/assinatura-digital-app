using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ITermsOfUseServices
    {
        Task<string> GetTermsUse();
    }
}
