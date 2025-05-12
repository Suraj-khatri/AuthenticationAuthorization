using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces;

public interface IRoleRepo : IGenericRepo<Role>
{
    // Add any additional methods specific to Role repository if needed
    Task<bool> IsExists(Role role);
}
