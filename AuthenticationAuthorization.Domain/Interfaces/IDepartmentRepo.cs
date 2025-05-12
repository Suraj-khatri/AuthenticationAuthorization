using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IDepartmentRepo : IGenericRepo<Department>
    {
        Task<bool> IsExists(Department department, CancellationToken cancellationToken = default);

    }
}
