using System;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface ICameraOverlayProvider
    {
        Func<object> GetImage(string imageName);
    }
}
