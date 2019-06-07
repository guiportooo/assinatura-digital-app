using AssinaturaDigital.Services.Interfaces;
using System;

namespace AssinaturaDigital.Droid.Services
{
    public class CameraOverlayProvider : ICameraOverlayProvider
    {
        public bool CanSetOverlay() => false;

        public Func<object> GetImage(string imageName)
        {
            throw new NotImplementedException();
        }
    }
}
