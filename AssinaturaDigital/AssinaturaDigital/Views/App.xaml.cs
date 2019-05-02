using AssinaturaDigital.Configuration;
using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Services.SignUp;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;

namespace AssinaturaDigital.Views
{
    public partial class App : PrismApplication
    {
        private bool _useFakes;

        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            var configurationManager = Container.Resolve<IConfigurationManager>();
            var config = configurationManager.Get();

            _useFakes = config.UseFakes;
            var iOSAppCenterSecret = config.IOSAppCenterSecret;
            var androidAppCenterSecret = config.AndroidAppCenterSecret;

            AppCenter.Start($"ios={iOSAppCenterSecret};android={androidAppCenterSecret}",
                  typeof(Analytics),
                  typeof(Crashes));

            NavigationService.NavigateAsync($"NavigationPage/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IErrorHandler, AppCenterCrashTracker>();
            containerRegistry.Register<IConfigurationManager, ConfigurationManager>();

            containerRegistry.Register<IDeviceTimer, DeviceTimer>();
            containerRegistry.Register<IPermissionsService, PermissionsService>();
            containerRegistry.Register<ICameraService, CameraService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<TokenPage, TokenViewModel>();
            containerRegistry.RegisterForNavigation<TermsOfUsePage, TermsOfUseViewModel>();
            containerRegistry.RegisterForNavigation<DocumentsSelectionPage, DocumentsSelectionViewModel>();
            containerRegistry.RegisterForNavigation<DocumentPage, DocumentViewModel>();
            containerRegistry.RegisterForNavigation<InfoSelfiePage, InfoSelfieViewModel>();
            containerRegistry.RegisterForNavigation<SelfiePage, SelfieViewModel>();
            containerRegistry.RegisterForNavigation<InfoRegisterPage, InfoRegisterViewModel>();

            containerRegistry.Register<ITokenService, TokenServiceFake>();
            containerRegistry.Register<ITermsOfUseServices, TermsOfUseServiceFake>();
            containerRegistry.Register<IDocumentsService, DocumentsServiceFake>();

            if (_useFakes)
            {
                containerRegistry.Register<ISignUpService, SignUpServiceFake>();
            }
            else
            {
                containerRegistry.Register<ISignUpService, SignUpService>();
            }
        }
    }
}
