using AssinaturaDigital.Services.Interfaces;
using System;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class CameraOverlayProviderMock : ICameraOverlayProvider
    {
        public string ImageName { get; private set; }
        public Func<object> Provider { get; private set; }

        public void SetProvider(Func<object> provider) => Provider = provider;

        public Func<object> GetImage(string imageName)
        {
            ImageName = imageName;
            return Provider;
        }
    }
}
