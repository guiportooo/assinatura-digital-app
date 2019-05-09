using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;

        public AuthenticationService(IConfigurationManager configurationManager, IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<AuthenticationResponse> SignUp(SignUpInformation signUpInformation)
        {
            try
            {
                var user = new User(0,
                    signUpInformation.FullName,
                    signUpInformation.CPF,
                    signUpInformation.CellPhoneNumber,
                    signUpInformation.Email);

                var createdUser = await _urlApi
                    .AppendPathSegment("users")
                    .PostJsonAsync(user)
                    .ReceiveJson<User>();

                return new AuthenticationResponse(createdUser);
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return await ex.GetResponseJsonAsync<AuthenticationResponse>();
            }
        }

        public async Task<AuthenticationResponse> GetByCPF(string cpf)
        {
            try
            {
                var createdUser = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment("cpf")
                    .AppendPathSegment(cpf)
                    .GetJsonAsync<User>();

                return new AuthenticationResponse(createdUser);
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return await ex.GetResponseJsonAsync<AuthenticationResponse>();
            }
        }
    }
}
