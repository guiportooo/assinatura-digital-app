using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Fakes;
using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;

namespace AssinaturaDigital
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent(); 
            NavigationService.NavigateAsync($"NavigationPage/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<TokenPage, TokenViewModel>();

            containerRegistry.Register<ISignUpService, SignUpServiceFake>();
        }
    }
}
