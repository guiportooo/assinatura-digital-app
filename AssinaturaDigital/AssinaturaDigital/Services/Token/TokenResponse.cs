using System.Net;

namespace AssinaturaDigital.Services.Token
{
    public class TokenResponse : ServiceResponse
    {
        public TokenResponse() => Status = (int)HttpStatusCode.OK;
    }
}
