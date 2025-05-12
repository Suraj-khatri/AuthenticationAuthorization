using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IBranchRepo : IGenericRepo<Branch>
    {
        Task<bool> IsExists(Branch branch, CancellationToken cancellationToken = default);

    }
}
