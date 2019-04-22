using AssinaturaDigital.Plugins;
using AssinaturaDigital.Services.Interfaces;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissions _permissions;

        public PermissionsService() => _permissions = Permissions.Instance;

        public async Task<bool> GrantedPermissionTo(Permission permission)
        {
            var status = await _permissions.CheckPermissionStatusAsync(permission);
            return status == PermissionStatus.Granted;
        }

        public async Task<bool> ShouldRequestPermissionTo(Permission permission)
            => await _permissions.ShouldShowRequestPermissionRationaleAsync(permission);

        public async Task<bool> RequestPermissionTo(Permission permission)
        {
            var permissions = await _permissions.RequestPermissionsAsync(permission);

            if (!permissions.ContainsKey(permission))
                return false;

            return permissions[permission] == PermissionStatus.Granted;
        }
    }
}
