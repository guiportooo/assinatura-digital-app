using AssinaturaDigital.Models;
using AssinaturaDigital.Utilities;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Services.Manifest
{
    public class ManifestService : IManifestService
    {
        private readonly IGeolocation _geolocation;
        private readonly IErrorHandler _errorHandler;

        public ManifestService(IGeolocation geolocation, IErrorHandler errorHandler)
        {
            _geolocation = geolocation;
            _errorHandler = errorHandler;
        }

        public async Task<ManifestInfos> Get()
        {
            var geolocation = await GetGeoLocation();
            return new ManifestInfos($"{geolocation.Latitude}, {geolocation.Longitude}", DateTime.Now);
        }

        private async Task<Location> GetGeoLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                return await _geolocation.GetLocationAsync(request);
            }
            catch (PermissionException ex)
            {
                _errorHandler.HandleError(ex);
                throw;
            }
        }
    }
}
