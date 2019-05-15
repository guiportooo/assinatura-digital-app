using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;

        public DocumentsService(IConfigurationManager configurationManager, IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task SaveDocument(Document document)
        {
            try
            {
                using (var photoStream = document.Photo.GetStream())
                {
                    var response = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(document.IdUser)
                    .AppendPathSegment("documents")
                    .PostMultipartAsync(x => x
                        .AddStringParts(new { document.Type, document.Orientation })
                        .AddFile("photo", photoStream, $"{document.Name}.JPG"));
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
