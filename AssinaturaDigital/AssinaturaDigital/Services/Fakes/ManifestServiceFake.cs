using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Manifest;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class ManifestServiceFake : IManifestService
    {
        public ManifestInfos ManifestInfos { get; private set; }

        public Task<ManifestInfos> Get() => Task.FromResult(new ManifestInfos("geolocation", DateTime.Now));
    }
}
