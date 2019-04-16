using AssinaturaDigital.Configuration;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class ConfigurationManagerMock : IConfigurationManager
    {
        private readonly Configuration.Configuration _configuration = new Configuration.Configuration
        {
            SecondsToGenerateToken = 60
        };

        public Configuration.Configuration Get() => _configuration;
    }
}
