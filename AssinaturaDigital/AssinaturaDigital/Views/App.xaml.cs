using AssinaturaDigital.Configuration;
using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Services.Selfies;
using AssinaturaDigital.Services.SignUp;
using AssinaturaDigital.Services.Token;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AssinaturaDigital.Views
{
    public partial class App : PrismApplication
    {
        private bool _useFakes;
        private string _iOSAppCenterSecret;
        private string _androidAppCenterSecret;

        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            AppCenter.Start($"ios={_iOSAppCenterSecret};android={_androidAppCenterSecret}",
                  typeof(Analytics),
                  typeof(Crashes));

            NavigationService.NavigateAsync($"NavigationPage/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IErrorHandler, AppCenterCrashTracker>();
            containerRegistry.Register<IConfigurationManager, ConfigurationManager>();

            GetConfigs();

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
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();

            containerRegistry.Register<ITermsOfUseServices, TermsOfUseServiceFake>();
            containerRegistry.Register<IDocumentsService, DocumentsServiceFake>();

            containerRegistry.RegisterInstance<IPreferences>(new PreferencesImplementation());

            if (_useFakes)
            {
                containerRegistry.Register<ISignUpService, SignUpServiceFake>();
                containerRegistry.Register<ITokenService, TokenServiceFake>();
                containerRegistry.Register<ISelfiesService, SelfiesServiceFake>();
            }
            else
            {
                containerRegistry.Register<ISignUpService, SignUpService>();
                containerRegistry.Register<ITokenService, TokenService>();
                containerRegistry.Register<ISelfiesService, SelfiesService>();
            }
        }

        private void GetConfigs()
        {
            var configurationManager = Container.Resolve<IConfigurationManager>();
            var config = configurationManager.Get();

            _useFakes = config.UseFakes;
            _iOSAppCenterSecret = config.IOSAppCenterSecret;
            _androidAppCenterSecret = config.AndroidAppCenterSecret;
        }
    }
}
