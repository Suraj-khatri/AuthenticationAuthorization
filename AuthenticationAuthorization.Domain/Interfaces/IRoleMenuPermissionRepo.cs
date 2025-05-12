using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IRoleMenuPermissionRepo : IGenericRepo<RoleMenuPermission>
    {
        Task<bool> IsExists(RoleMenuPermission roleMenuPermission);
    }
}
