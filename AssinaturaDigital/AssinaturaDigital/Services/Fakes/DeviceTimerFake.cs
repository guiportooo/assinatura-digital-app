using AssinaturaDigital.Services.Interfaces;
using System;

namespace AssinaturaDigital.Services.Fakes
{
    public class DeviceTimerFake : IDeviceTimer
    {
        public int Seconds { get; private set; }
        public Func<bool> Callback { get; private set; }

        public void Start(int seconds, Func<bool> callback)
        {
            Seconds = seconds;
            Callback = callback;
        }
    }
}
