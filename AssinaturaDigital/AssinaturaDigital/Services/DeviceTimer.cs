using AssinaturaDigital.Services.Interfaces;
using System;
using Xamarin.Forms;

namespace AssinaturaDigital.Services
{
    public class DeviceTimer : IDeviceTimer
    {
        public void Start(int seconds, Func<bool> callback) 
            => Device.StartTimer(TimeSpan.FromSeconds(seconds), callback);
    }
}
