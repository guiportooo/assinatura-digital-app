using Prism.Navigation;
using System.Linq;

namespace AssinaturaDigital.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static void RemoveLastViewWithName(this INavigationService navigationService, string name)
        {
            var actualPage = ((Prism.Common.IPageAware)navigationService).Page;
            var pageType = PageNavigationRegistry.GetPageType(name);
            var lastView = actualPage.Navigation.NavigationStack.LastOrDefault(p => p.GetType() == pageType);

            if (lastView == null)
                return;

            actualPage.Navigation.RemovePage(lastView);
        }
    }
}
