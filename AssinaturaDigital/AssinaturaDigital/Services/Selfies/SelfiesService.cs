using AssinaturaDigital.Configuration;
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
                using (var photoStream = photo.GetStream())
                {
                    var response = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(idUser)
                    .AppendPathSegment("selfies")
                    .PostMultipartAsync(x => x.AddFile("photo", photoStream, $"{idUser}_Selfie.JPG"));
                }

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
