using System;
using System.IO;
using System.Threading.Tasks;

namespace AssinaturaDigital.Configuration
{
    public interface IConfigurationStreamProvider : IDisposable
    {
        Task<Stream> GetStream();
    }
}
