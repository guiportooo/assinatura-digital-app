using Prism.Navigation;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class NavigationServiceMock : PageNavigationService
    {
        public string Name { get; private set; }
        public INavigationParameters Parameters { get; private set; }
        public bool? UseModalNavigation { get; private set; }
        public bool Animated { get; private set; }

        public NavigationServiceMock() : base(null, null, null, null) { }

        public override Task<INavigationResult> NavigateAsync(string name)
        {
            Name = name;
            INavigationResult result = new NavigationResult();
            return Task.FromResult(result);
        }

        protected override Task<INavigationResult> NavigateInternal(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            Name = name;
            Parameters = parameters;
            UseModalNavigation = useModalNavigation;
            Animated = animated;
            INavigationResult result = new NavigationResult();
            return Task.FromResult(result);
        }
    }
}
