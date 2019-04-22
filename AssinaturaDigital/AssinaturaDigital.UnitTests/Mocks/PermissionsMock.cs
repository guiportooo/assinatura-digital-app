using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class PermissionsMock : IPermissions
    {
        private readonly Dictionary<Permission, PermissionStatus> _defaultPermissions;
        private readonly Dictionary<Permission, bool> _shouldRequestPermissions;

        public PermissionsMock(Dictionary<Permission, PermissionStatus> defaultPermissions) 
            => _defaultPermissions = defaultPermissions;

        public PermissionsMock(Dictionary<Permission, bool> shouldRequestPermissions) 
            => _shouldRequestPermissions = shouldRequestPermissions;

        public Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission) 
            => Task.FromResult(_defaultPermissions[permission]);

        public Task<bool> ShouldShowRequestPermissionRationaleAsync(Permission permission)
            => Task.FromResult(_shouldRequestPermissions[permission]);

        public Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions) 
            => Task.FromResult(_defaultPermissions);

        public bool OpenAppSettings()
        {
            throw new NotImplementedException();
        }
    }
}
