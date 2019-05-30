using AssinaturaDigital.Models;
using System.Net;

namespace AssinaturaDigital.Services.Authentication
{
    public class AuthenticationResponse : ServiceResponse
    {
        public SignUpInformation SignUpInformation { get; set; }

        public bool UserInterruptedSignUp => Status == (int)HttpStatusCode.Forbidden;

        public AuthenticationResponse() { }

        public AuthenticationResponse(User user, int status)
        {
            Status = status;

            if (user == null)
                return;

            SignUpInformation = new SignUpInformation(user.Id,
                user.FullName,
                user.CPF,
                user.CellPhoneNumber,
                user.Email);
        }
    }
}
