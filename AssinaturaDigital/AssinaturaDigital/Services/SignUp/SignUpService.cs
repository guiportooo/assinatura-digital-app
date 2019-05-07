using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.SignUp
{
    public class SignUpService : ISignUpService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;

        public SignUpService(IConfigurationManager configurationManager, IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<SignUpResponse> SignUp(SignUpInformation signUpInformation)
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

                return new SignUpResponse(createdUser);
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return await ex.GetResponseJsonAsync<SignUpResponse>();
            }
        }

        public async Task<SignUpResponse> GetByCPF(string cpf)
        {
            try
            {
                var createdUser = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment("cpf")
                    .AppendPathSegment(cpf)
                    .GetJsonAsync<User>();

                return new SignUpResponse(createdUser);
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return await ex.GetResponseJsonAsync<SignUpResponse>();
            }
        }
    }
}
