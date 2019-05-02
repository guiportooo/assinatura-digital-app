using AssinaturaDigital.Models;
using System.Net;

namespace AssinaturaDigital.Services.SignUp
{
    public class SignUpResponse : ServiceResponse
    {
        public SignUpInformation SignUpInformation { get; set; }

        public SignUpResponse() { }

        public SignUpResponse(User user)
        {
            Status = (int)HttpStatusCode.OK;
            SignUpInformation = new SignUpInformation(user.FullName,
                           user.CPF,
                           user.CellPhoneNumber,
                           user.Email);
        }
    }
}
