using AssinaturaDigital.Configuration;
using AssinaturaDigital.Models;
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

        public ContractsService(IConfigurationManager configurationManager, IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

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

        public async Task<bool> SignContract(int id, int idUser, MediaFile photo)
        {
            try
            {
                using (var photoStream = photo.GetStream())
                {
                    var content = new CapturedMultipartContent();
                    content.AddStringParts(new { id });
                    content.AddFile("photo", photoStream, $"{idUser}_{id}_Selfie.JPG");

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
