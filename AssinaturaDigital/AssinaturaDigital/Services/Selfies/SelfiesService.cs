using AssinaturaDigital.Configuration;
using AssinaturaDigital.Extensions;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Selfies
{
    public class SelfiesService : ISelfiesService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;

        public SelfiesService(IConfigurationManager configurationManager, IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<bool> SaveSelfie(int idUser, MediaFile photo)
        {
            try
            {
                var base64Photo = await photo.GetStream().ToBase64();
                var userSelfie = new UserSelfie(idUser, base64Photo);

                var response = await _urlApi
                    .AppendPathSegment("selfies")
                    .AppendPathSegment("validate-user")
                    .PostJsonAsync(userSelfie);

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
