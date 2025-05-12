using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IPermissionRepo : IGenericRepo<Permission>
    {
        Task<bool> IsExists(Permission permission);
    }
}
