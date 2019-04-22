using System;
namespace AssinaturaDigital.Services.Interfaces
{
    public interface IDeviceTimer
    {
        void Start(int seconds, Func<bool> callback);
    }
}
