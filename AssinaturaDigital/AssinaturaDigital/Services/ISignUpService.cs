using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public interface ISignUpService
    {
        Task SignUp(SignUpInformation signUpInformation);
    }
}
