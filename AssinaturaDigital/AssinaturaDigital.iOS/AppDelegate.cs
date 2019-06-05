using AssinaturaDigital.Configuration;
using AssinaturaDigital.iOS.Configuration;
using AssinaturaDigital.iOS.Services;
using AssinaturaDigital.Services.Interfaces;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;

namespace AssinaturaDigital.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            var xfApplication = new Views.App(new IOSInitializer());
#if DEBUG
            HotReloader.Current.Start(xfApplication, 4291);
#endif
            LoadApplication(xfApplication);
            KeyboardOverlapRenderer.Init();
            return base.FinishedLaunching(app, options);
        }
    }

    public class IOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IConfigurationStreamProviderFactory, IOSConfigurationStreamProviderFactory>();
            containerRegistry.Register<ICameraOverlayProvider, CameraOverlayProvider>();
        }
    }
}
