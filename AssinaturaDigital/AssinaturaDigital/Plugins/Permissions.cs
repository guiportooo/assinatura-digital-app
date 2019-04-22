using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace AssinaturaDigital.Plugins
{

    public static class Permissions
    {
        private static IPermissions _permissions;

        public static IPermissions Instance
        {
            get => _permissions ?? (_permissions = CrossPermissions.Current);
            set => _permissions = value;
        }
    }
}
