using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken();
        Task<bool> ValidateToken(string token);
    }
}
