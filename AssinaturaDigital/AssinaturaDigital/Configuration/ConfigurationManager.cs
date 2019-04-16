using AssinaturaDigital.Extensions;
using AssinaturaDigital.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AssinaturaDigital.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationStreamProviderFactory _factory;
        private Configuration _configuration;

        public ConfigurationManager(IConfigurationStreamProviderFactory factory, IErrorHandler errorHandler)
        {
            _factory = factory;
            InitializeConfiguration().FireAndForget(errorHandler);
        }

        private async Task InitializeConfiguration() => _configuration = await Read();

        private async Task<Configuration> Read()
        {
            using (var streamProvider = _factory.Create())
            using (var stream = await streamProvider.GetStream().ConfigureAwait(false))
                return Deserialize<Configuration>(stream);
        }

        private T Deserialize<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
                return new JsonSerializer().Deserialize<T>(jtr);
        }

        public Configuration Get()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration should not be null");

            return _configuration;
        }
    }
}
