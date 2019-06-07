using AssinaturaDigital.Services.Interfaces;
using AVFoundation;
using CoreMedia;
using Foundation;
using System.IO;
using UIKit;

namespace AssinaturaDigital.iOS.Services
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        public Stream GenerateThumbImage(string url, long usecond)
        {
            var imageGenerator = new AVAssetImageGenerator(AVAsset.FromUrl(new NSUrl(url)))
            {
                AppliesPreferredTrackTransform = true
            };

            var cgImage = imageGenerator.CopyCGImageAtTime(new CMTime(usecond, 1000000), out var actualTime, out var error);

            return new UIImage(cgImage).AsJPEG().AsStream();
        }
    }
}
