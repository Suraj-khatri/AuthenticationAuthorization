using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class UserTokenRepo : GenericRepo<UserToken>, IUserToken
    {
        private readonly AppDbContext _context;
        public UserTokenRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        // Add any specific methods for UserToken here
    }

}
