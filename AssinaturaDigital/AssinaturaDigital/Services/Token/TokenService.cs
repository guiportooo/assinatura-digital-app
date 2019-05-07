using AssinaturaDigital.Configuration;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;

        public TokenService(IConfigurationManager configurationManager,
            IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;

        }
        public async Task<TokenResponse> GenerateToken(int idUser)
        {
            try
            {
                var tokenReponse = await _urlApi
                    .AppendPathSegment("tokens")
                    .AppendPathSegment("send-by-id")
                    .PostJsonAsync(idUser);

                return new TokenResponse();
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return await ex.GetResponseJsonAsync<TokenResponse>();
            }
        }
        public async Task<bool> ValidateToken(int idUser, string token)
        {
            try
            {
                var request = new ValidateTokenRequest(idUser, token);

                var response = await _urlApi
                    .AppendPathSegment("tokens")
                    .AppendPathSegment("validate")
                    .PostJsonAsync(request);

                return true;
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return false;
            }
        }
    }
}
