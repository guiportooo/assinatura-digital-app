using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.SignUp
{
    public interface ISignUpService
    {
        Task<SignUpResponse> SignUp(SignUpInformation signUpInformation);
    }
}
