using Android.App;
using Android.Content;
using AssinaturaDigital.Configuration;

namespace AssinaturaDigital.Droid.Configuration
{
    public class AndroidConfigurationStreamProviderFactory : IConfigurationStreamProviderFactory
    {
        private readonly Context _context;

        public AndroidConfigurationStreamProviderFactory() => _context = Application.Context;

        public IConfigurationStreamProvider Create() => new AndroidConfigurationStreamProvider(_context);
    }
}
