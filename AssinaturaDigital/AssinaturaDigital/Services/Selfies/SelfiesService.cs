using AssinaturaDigital.Configuration;
using AssinaturaDigital.Extensions;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Services.Selfies
{
    public class SelfiesService : ISelfiesService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;
        private readonly IDeviceInfo _deviceInfo;

        public SelfiesService(IConfigurationManager configurationManager, 
            IErrorHandler errorHandler,
            IDeviceInfo deviceInfo)
        {
            _errorHandler = errorHandler;
            _deviceInfo = deviceInfo;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<bool> SaveSelfie(int idUser, MediaFile photo)
        {
            try
            {
                using (var photoStream = photo.GetStreamWithCorrectRotation(_deviceInfo))
                {
                    var response = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(idUser)
                    .AppendPathSegment("selfies")
                    .PostMultipartAsync(x => x.AddFile("photo", photoStream, $"{idUser}_Selfie.PNG"));
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
