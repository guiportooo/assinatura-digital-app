using AssinaturaDigital.Services.Interfaces;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace AssinaturaDigital.iOS.Services
{
    public class CameraOverlayProvider : ICameraOverlayProvider
    {
        protected UIImageView ImageView { get; set; }
        private NSObject _observeEvent;

        const string photoPreviewShowEvent = "_UIImagePickerControllerUserDidCaptureItem";
        const string retakeEvent = "_UIImagePickerControllerUserDidRejectItem";
        const string cameraSessionStopEvent = "AVCaptureSessionDidStopRunningNotification";

        public bool CanSetOverlay() => true;

        public Func<object> GetImage(string imageName)
        {
            var screen = UIScreen.MainScreen.Bounds;
            var screenWidth = screen.Size.Width;
            var screenHeight = screen.Size.Height;

            var frame = new CGRect((int)(screen.GetMidX() - screenWidth / 2),
                (int)(screen.GetMidY() - screenHeight / 2),
                screenWidth,
                screenHeight * 0.8);

            ImageView = new UIImageView(frame)
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                Image = UIImage.FromBundle(imageName)
            };

            _observeEvent = NSNotificationCenter.DefaultCenter.AddObserver(null, HandleNotificationSettings);

            return () => ImageView;
        }

        void HandleNotificationSettings(NSNotification obj)
        {
            switch (obj.Name)
            {
                case photoPreviewShowEvent:
                    if (ImageView != null)
                        ImageView.Hidden = true;
                    break;
                case retakeEvent:
                    if (ImageView != null)
                        ImageView.Hidden = false;
                    break;
                case cameraSessionStopEvent:
                    if (_observeEvent != null)
                        NSNotificationCenter.DefaultCenter.RemoveObserver(_observeEvent);
                    break;
            }
        }
    }
}
