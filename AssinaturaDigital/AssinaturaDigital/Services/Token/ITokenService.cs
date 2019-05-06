using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateToken(int idUser);
        Task<bool> ValidateToken(int idUser, string token);
    }
}
