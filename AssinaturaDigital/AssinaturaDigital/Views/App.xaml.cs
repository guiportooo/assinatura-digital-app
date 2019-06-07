using AssinaturaDigital.Configuration;
using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Authentication;
using AssinaturaDigital.Services.Contracts;
using AssinaturaDigital.Services.Documents;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Services.Manifest;
using AssinaturaDigital.Services.Token;
using AssinaturaDigital.Services.Validations;
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
            containerRegistry.RegisterForNavigation<VideoPage, VideoViewModel>();
            containerRegistry.RegisterForNavigation<InfoRegisterPage, InfoRegisterViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<ContractListPage, ContractListViewModel>();
            containerRegistry.RegisterForNavigation<ContractDetailPage, ContractDetailViewModel>();
            containerRegistry.RegisterForNavigation<InfoSigningContractPage, InfoSigningContractViewModel>();
            containerRegistry.RegisterForNavigation<CNHOrientationPage, CNHOrientationViewModel>();
            containerRegistry.RegisterForNavigation<RGOrientationPage, RGOrientationViewModel>();
            containerRegistry.RegisterForNavigation<SelfieOrientationPage, SelfieOrientationViewModel>();

            containerRegistry.Register<ITermsOfUseServices, TermsOfUseServiceFake>();

            containerRegistry.RegisterInstance<IPreferences>(new PreferencesImplementation());
            containerRegistry.RegisterInstance<IDeviceInfo>(new DeviceInfoImplementation());
            containerRegistry.RegisterInstance<IGeolocation>(new GeolocationImplementation());

            if (_useFakes)
            {
                containerRegistry.Register<IAuthenticationService, AuthenticationServiceFake>();
                containerRegistry.Register<ITokenService, TokenServiceFake>();
                containerRegistry.Register<IValidationsService, ValidationsServiceFake>();
                containerRegistry.Register<IDocumentsService, DocumentsServiceFake>();
                containerRegistry.Register<IContractsService, ContractsServiceFake>();
            }
            else
            {
                containerRegistry.Register<IAuthenticationService, AuthenticationService>();
                containerRegistry.Register<ITokenService, TokenService>();
                containerRegistry.Register<IValidationsService, ValidationsService>();
                containerRegistry.Register<IDocumentsService, DocumentsService>();
                containerRegistry.Register<IContractsService, ContractsService>();
                containerRegistry.Register<IManifestService, ManifestService>();
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
