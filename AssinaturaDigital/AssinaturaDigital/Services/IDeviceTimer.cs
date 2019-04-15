using System;
namespace AssinaturaDigital.Services
{
    public interface IDeviceTimer
    {
        void Start(int seconds, Func<bool> callback);
    }
}
