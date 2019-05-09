using AssinaturaDigital.Models;
using System.Net;

namespace AssinaturaDigital.Services.Authentication
{
    public class AuthenticationResponse : ServiceResponse
    {
        public SignUpInformation SignUpInformation { get; set; }

        public AuthenticationResponse() { }

        public AuthenticationResponse(User user)
        {
            if (user == null)
                return;

            Status = (int)HttpStatusCode.OK;

            SignUpInformation = new SignUpInformation(user.Id,
                user.FullName,
                user.CPF,
                user.CellPhoneNumber,
                user.Email);
        }
    }
}
