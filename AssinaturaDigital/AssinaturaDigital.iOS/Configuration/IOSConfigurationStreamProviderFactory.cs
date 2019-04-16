using AssinaturaDigital.Configuration;

namespace AssinaturaDigital.iOS.Configuration
{
    public class IOSConfigurationStreamProviderFactory : IConfigurationStreamProviderFactory
    {
        public IConfigurationStreamProvider Create() => new IOSConfigurationStreamProvider();
    }
}
