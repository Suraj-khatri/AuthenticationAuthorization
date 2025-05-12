using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User?>GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}
