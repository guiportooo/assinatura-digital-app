using AssinaturaDigital.Plugins;
using AssinaturaDigital.Services;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.UnitTests.Mocks;
using FluentAssertions;
using NUnit.Framework;
using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Services
{
    public class PermissionsServiceTests
    {
        private PermissionsService _permissionsService;

        [TestCase(PermissionStatus.Granted, true)]
        [TestCase(PermissionStatus.Denied, false)]
        [TestCase(PermissionStatus.Disabled, false)]
        [TestCase(PermissionStatus.Restricted, false)]
        [TestCase(PermissionStatus.Unknown, false)]
        public async Task ShouldInformIfPermissionIsGranted(PermissionStatus status, bool isGranted)
        {
            var permission = Permission.Camera;

            var permissions = new Dictionary<Permission, PermissionStatus>
            {
                {  permission, status }
            };

            var permissionsMock = new PermissionsMock(permissions);
            Permissions.Instance = permissionsMock;

            _permissionsService = new PermissionsService();

            var permissionIsGranted = await _permissionsService.GrantedPermissionTo(permission);

            permissionIsGranted.Should().Be(isGranted);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldInfomrIfShouldRequestPermission(bool shouldRequest)
        {
            var permission = Permission.Camera;

            var permissions = new Dictionary<Permission, bool>
            {
                { permission, shouldRequest }
            };

            var permissionsMock = new PermissionsMock(permissions);
            Permissions.Instance = permissionsMock;

            _permissionsService = new PermissionsService();

            var shouldRequestPermission = await _permissionsService.ShouldRequestPermissionTo(permission);

            shouldRequestPermission.Should().Be(shouldRequest);
        }

        [Test]
        public async Task WhenRequestingPermissionShouldInfomPermissionNotGrantedIfPermissionDoesntExist()
        {
            var askedPermission = Permission.Camera;
            var existingPermission = Permission.Location;

            var permissions = new Dictionary<Permission, PermissionStatus>
            {
                {  existingPermission, PermissionStatus.Granted }
            };

            var permissionsMock = new PermissionsMock(permissions);
            Permissions.Instance = permissionsMock;

            _permissionsService = new PermissionsService();

            var grantedPermission = await _permissionsService.RequestPermissionTo(askedPermission);

            grantedPermission.Should().Be(false);
        }

        [TestCase(PermissionStatus.Granted, true)]
        [TestCase(PermissionStatus.Denied, false)]
        [TestCase(PermissionStatus.Disabled, false)]
        [TestCase(PermissionStatus.Restricted, false)]
        [TestCase(PermissionStatus.Unknown, false)]
        public async Task WhenRequestingPermissionShouldInformIfPermissionIsGranted(PermissionStatus status, bool isGranted)
        {
            var permission = Permission.Camera;

            var permissions = new Dictionary<Permission, PermissionStatus>
            {
                {  permission, status }
            };

            var permissionsMock = new PermissionsMock(permissions);
            Permissions.Instance = permissionsMock;

            _permissionsService = new PermissionsService();

            var permissionIsGranted = await _permissionsService.RequestPermissionTo(permission);

            permissionIsGranted.Should().Be(isGranted);
        }
    }
}
