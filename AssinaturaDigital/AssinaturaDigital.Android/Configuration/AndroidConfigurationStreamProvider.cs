using Android.Content;
using AssinaturaDigital.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AssinaturaDigital.Droid.Configuration
{
    public class AndroidConfigurationStreamProvider : IConfigurationStreamProvider
    {
        private const string configurationFilePath = "config.json";
        private readonly Context _context;
        private Stream _readingStream;

        public AndroidConfigurationStreamProvider(Context context) => _context = context;

        public Task<Stream> GetStream()
        {
            ReleaseUnmanagedResources();
            var assets = _context.Assets;
            _readingStream = assets.Open(configurationFilePath);
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

        ~AndroidConfigurationStreamProvider()
        {
            ReleaseUnmanagedResources();
        }
    }
}
