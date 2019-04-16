using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public interface ITermsOfUseServices
    {
        Task<string> GetTermsUse();
    }
}
