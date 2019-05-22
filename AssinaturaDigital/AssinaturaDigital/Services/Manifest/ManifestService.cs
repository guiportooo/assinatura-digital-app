using AssinaturaDigital.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Services.Manifest
{
    public class ManifestService : IManifestService
    {
        private readonly IGeolocation _geolocation;

        public ManifestService(IGeolocation geolocation) => _geolocation = geolocation;

        public async Task<ManifestInfos> Get()
        {
            var geolocation = await GetGeoLocation();
            return new ManifestInfos($"{geolocation.Latitude}, {geolocation.Longitude}", DateTime.Now);
        }

        private async Task<Location> GetGeoLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            return await _geolocation.GetLocationAsync(request);
        }
    }
}
