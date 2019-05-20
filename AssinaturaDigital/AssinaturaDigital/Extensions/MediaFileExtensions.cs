using Plugin.Media.Abstractions;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.Extensions
{
    public static class MediaFileExtensions
    {
        public static Stream GetStreamWithCorrectRotation(this MediaFile photo, IDeviceInfo deviceInfo)
        {
            if (deviceInfo.Platform.Equals(DevicePlatform.iOS))
                return photo.GetStreamWithImageRotatedForExternalStorage();

            return photo.GetStream();
        }
    }
}
