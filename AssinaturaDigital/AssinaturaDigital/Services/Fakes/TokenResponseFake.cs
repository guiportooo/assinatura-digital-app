using AssinaturaDigital.Services.Token;

namespace AssinaturaDigital.Services.Fakes
{
    public class TokenResponseFake : TokenResponse
    {
        public string Token { get; }

        public TokenResponseFake(string token) => Token = token;
    }
}
