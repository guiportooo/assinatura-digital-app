using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AssinaturaDigital.Configuration;
using AssinaturaDigital.Droid.Configuration;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace AssinaturaDigital.Droid
{
    [Activity(Label = "VerifiKey", Icon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            var xfApplication = new Views.App(new AndroidInitializer());
#if DEBUG
            HotReloader.Current.Start(xfApplication, 8000);
#endif
            LoadApplication(xfApplication);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
                => containerRegistry.Register<IConfigurationStreamProviderFactory, AndroidConfigurationStreamProviderFactory>();
        }
    }
}