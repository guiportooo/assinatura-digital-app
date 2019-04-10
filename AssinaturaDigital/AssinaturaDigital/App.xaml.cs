using AssinaturaDigital.ViewModels;
using AssinaturaDigital.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;

namespace AssinaturaDigital
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync(nameof(MainPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
        }
    }
}
