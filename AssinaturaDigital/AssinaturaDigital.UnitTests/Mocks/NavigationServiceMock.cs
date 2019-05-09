using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class NavigationServiceMock : PageNavigationService
    {
        private bool _shouldFail;

        public string Name { get; private set; }
        public INavigationParameters Parameters { get; private set; }
        public bool? UseModalNavigation { get; private set; }
        public bool Animated { get; private set; }
        public bool WentBack { get; private set; }

        public NavigationServiceMock() : base(null, null, null, null) { }

        public void ShouldFail(bool shouldFail) => _shouldFail = shouldFail;

        public override Task<INavigationResult> NavigateAsync(string name)
        {
            if (_shouldFail)
                throw new Exception("Failed to navigate.");

            Name = name;
            INavigationResult result = new NavigationResult();
            return Task.FromResult(result);
        }

        protected override Task<INavigationResult> NavigateInternal(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            if (_shouldFail)
                throw new Exception("Failed to navigate.");

            Name = name;
            Parameters = parameters;
            UseModalNavigation = useModalNavigation;
            Animated = animated;
            INavigationResult result = new NavigationResult();
            return Task.FromResult(result);
        }

        public override Task<INavigationResult> GoBackAsync()
        {
            if (_shouldFail)
                throw new Exception("Failed to go back.");

            WentBack = true;
            INavigationResult result = new NavigationResult();
            return Task.FromResult(result);
        }
    }
}
