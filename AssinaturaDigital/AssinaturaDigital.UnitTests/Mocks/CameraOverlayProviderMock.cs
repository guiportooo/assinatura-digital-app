using AssinaturaDigital.Services.Interfaces;
using System;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class CameraOverlayProviderMock : ICameraOverlayProvider
    {
        private bool _canSetOverlay;
        public string ImageName { get; private set; }
        public Func<object> Provider { get; private set; }

        public void ShouldSetOverlay(bool shouldSetOverlay) => _canSetOverlay = shouldSetOverlay;

        public void SetProvider(Func<object> provider) => Provider = provider;

        public bool CanSetOverlay() => _canSetOverlay;

        public Func<object> GetImage(string imageName)
        {
            if (!_canSetOverlay)
                return null;

            ImageName = imageName;
            return Provider;
        }
    }
}
