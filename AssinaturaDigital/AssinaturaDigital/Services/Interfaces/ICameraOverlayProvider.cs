using System;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ICameraOverlayProvider
    {
        bool CanSetOverlay();
        Func<object> GetImage(string imageName);
    }
}
