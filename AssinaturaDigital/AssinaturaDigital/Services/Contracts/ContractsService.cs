using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Contracts
{
    public class ContractsService : IContractsService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;
        private readonly IThumbnailGenerator _thumbnailGenerator;

        public ContractsService(IConfigurationManager configurationManager,
            IErrorHandler errorHandler,
            IThumbnailGenerator thumbnailGenerator)
        {
            _errorHandler = errorHandler;
            _thumbnailGenerator = thumbnailGenerator;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<IList<ContractData>> GetContracts(int idUser)
        {
            try
            {
                var contracts = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(idUser)
                    .AppendPathSegment("contracts")
                    .GetJsonAsync<IEnumerable<ContractData>>();

                return contracts.ToList();
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                throw ex;
            }
        }

        public async Task<bool> SignContract(int id, int idUser, MediaFile video, ManifestInfos manifestInfos)
        {
            try
            {
                using (var videoStream = video.GetStream())
                {
                    var selfieStream = _thumbnailGenerator.GenerateThumbImage(video.AlbumPath, 1);

                    var signatureDate = manifestInfos.SignatureDate.ToLongDateString();
                    var content = new CapturedMultipartContent();

                    content.AddFile("video", videoStream, $"{idUser}_{id}_Video.mp4");
                    content.AddFile("selfie", selfieStream, $"{idUser}_{id}_Selfie.png");
                    content.AddStringParts(new { manifestInfos.GeoLocation, signatureDate });

                    var response = await _urlApi
                        .AppendPathSegment("users")
                        .AppendPathSegment(idUser)
                        .AppendPathSegment("contracts")
                        .AppendPathSegment(id)
                        .SendAsync(HttpMethod.Put, content);
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
