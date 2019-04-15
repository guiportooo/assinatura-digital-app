using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken();
        Task<bool> ValidateToken(string token);
    }
}
