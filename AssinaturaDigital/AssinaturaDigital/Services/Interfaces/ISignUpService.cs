using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ISignUpService
    {
        Task SignUp(SignUpInformation signUpInformation);
    }
}
