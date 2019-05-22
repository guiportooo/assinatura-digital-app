using AssinaturaDigital.Configuration;
using AssinaturaDigital.Extensions;
using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Manifest;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Services.Contracts
{
    public class ContractsService : IContractsService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;
        private readonly IDeviceInfo _deviceInfo;

        public ContractsService(IConfigurationManager configurationManager, 
            IErrorHandler errorHandler,
            IDeviceInfo deviceInfo)
        {
            _errorHandler = errorHandler;
            _deviceInfo = deviceInfo;

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

        public async Task<bool> SignContract(int id, int idUser, MediaFile photo, ManifestInfos manifestInfos)
        {
            try
            {
                using (var photoStream = photo.GetStreamWithCorrectRotation(_deviceInfo))
                {
                    var signatureDate = manifestInfos.SignatureDate.ToLongDateString();
                    var content = new CapturedMultipartContent();

                    content.AddFile("photo", photoStream, $"{idUser}_{id}_Selfie.PNG");
                    content.AddStringParts(new {manifestInfos.GeoLocation, signatureDate });

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
