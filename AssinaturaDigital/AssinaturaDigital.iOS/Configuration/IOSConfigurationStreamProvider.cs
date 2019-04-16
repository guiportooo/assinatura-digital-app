using AssinaturaDigital.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AssinaturaDigital.iOS.Configuration
{
    public class IOSConfigurationStreamProvider : IConfigurationStreamProvider
    {
        private const string configurationFilePath = "Assets/config.json";
        private Stream _readingStream;

        public Task<Stream> GetStream()
        {
            ReleaseUnmanagedResources();
            _readingStream = new FileStream(configurationFilePath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(_readingStream);
        }

        private void ReleaseUnmanagedResources()
        {
            _readingStream?.Dispose();
            _readingStream = null;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~IOSConfigurationStreamProvider()
        {
            ReleaseUnmanagedResources();
        }
    }
}
