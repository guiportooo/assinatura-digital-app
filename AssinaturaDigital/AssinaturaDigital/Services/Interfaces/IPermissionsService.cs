using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface IPermissionsService
    {
        Task<bool> GrantedPermissionTo(Permission permission);
        Task<bool> ShouldRequestPermissionTo(Permission permission);
        Task<bool> RequestPermissionTo(Permission permission);
    }
}
