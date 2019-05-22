using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Manifest
{
    public interface IManifestService
    {
        Task<ManifestInfos> Get();
    }
}
