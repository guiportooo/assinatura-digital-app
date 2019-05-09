using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> SignUp(SignUpInformation signUpInformation);

        Task<AuthenticationResponse> GetByCPF(string cpf);
    }
}
