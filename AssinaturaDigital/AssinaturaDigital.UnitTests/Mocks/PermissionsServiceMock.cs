using AssinaturaDigital.Services.Interfaces;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class PermissionsServiceMock : IPermissionsService
    {
        private bool _grantedPermissionBeforeRequest;
        private bool _shouldRequestPermission;
        private bool _grantedPermissionAfterRequest;
        public Permission Permission { get; private set; }

        public void GrantedPermissionBeforeRequest() => _grantedPermissionBeforeRequest = true;

        public void GrantedPermissionAfterRequest()
        {
            _shouldRequestPermission = true;
            _grantedPermissionAfterRequest = true;
        }

        public Task<bool> GrantedPermissionTo(Permission permission)
        {
            Permission = permission;
            return Task.FromResult(_grantedPermissionBeforeRequest);
        }

        public Task<bool> ShouldRequestPermissionTo(Permission permission)
        {
            Permission = permission;
            return Task.FromResult(_shouldRequestPermission);
        }

        public Task<bool> RequestPermissionTo(Permission permission)
        {
            Permission = permission;
            return Task.FromResult(_grantedPermissionAfterRequest);
        }
    }
}
