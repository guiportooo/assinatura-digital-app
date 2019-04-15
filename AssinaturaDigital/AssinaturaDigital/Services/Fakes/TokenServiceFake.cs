using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class TokenServiceFake : ITokenService
    {
        private string _fakeToken;
        private Exception _exception;

        public string PassedToken { get; set; }

        public Task<string> GenerateToken()
        {
            _fakeToken = new Random().Next(100000, 999999).ToString();
            return Task.FromResult(_fakeToken);
        }

        public Task<bool> ValidateToken(string token)
        {
            PassedToken = token;

            if (_exception != null)
                throw _exception;

            return Task.FromResult(token.Equals(_fakeToken));
        }

        public void ShouldRaiseException(Exception ex) => _exception = ex;
    }
}
