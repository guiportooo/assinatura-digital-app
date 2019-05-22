using AssinaturaDigital.Configuration;
using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Services.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;
        private readonly IDeviceInfo _deviceInfo;

        public DocumentsService(IConfigurationManager configurationManager, 
            IErrorHandler errorHandler,
            IDeviceInfo deviceInfo)
        {
            _errorHandler = errorHandler;
            _deviceInfo = deviceInfo;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task SaveDocument(Document document)
        {
            try
            {
                using (var photoStream = document.Photo.GetStreamWithCorrectRotation(_deviceInfo))
                {
                    var response = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(document.IdUser)
                    .AppendPathSegment("documents")
                    .PostMultipartAsync(x => x
                        .AddStringParts(new { document.Type, document.Orientation })
                        .AddFile("photo", photoStream, $"{document.Name}.PNG"));
                }
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}
