using AssinaturaDigital.Services.Token;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class TokenServiceFake : ITokenService
    {
        private string _fakeToken;
        private Exception _exception;

        public int PassedIdUser { get; private set; }
        public string PassedToken { get; private set; }

        public void ShouldRaiseException(Exception ex) => _exception = ex;

        public Task<TokenResponse> GenerateToken(int idUser)
        {
            _fakeToken = new Random().Next(100000, 999999).ToString();
            TokenResponse response = new TokenResponseFake(_fakeToken);
            return Task.FromResult(response);
        }

        public Task<bool> ValidateToken(int idUser, string token)
        {
            PassedIdUser = idUser;
            PassedToken = token;

            if (_exception != null)
                throw _exception;

            return Task.FromResult(token.Equals(_fakeToken));
        }
    }
}
