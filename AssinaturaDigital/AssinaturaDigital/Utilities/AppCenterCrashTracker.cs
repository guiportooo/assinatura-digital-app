using Microsoft.AppCenter.Crashes;
using System;

namespace AssinaturaDigital.Utilities
{
    public class AppCenterCrashTracker : IErrorHandler
    {
        public void HandleError(Exception ex) => Crashes.TrackError(ex);
    }
}
