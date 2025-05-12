using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IMenuPermissionRepo : IGenericRepo<MenuPermission>
    {
        Task<bool> IsExists(MenuPermission menuPermission);
    }

}
