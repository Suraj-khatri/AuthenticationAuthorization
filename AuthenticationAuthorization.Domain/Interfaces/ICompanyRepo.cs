using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface ICompanyRepo : IGenericRepo<Company>
    {
        Task<bool> IsExists(Company company, CancellationToken cancellationToken = default);
    }
}
