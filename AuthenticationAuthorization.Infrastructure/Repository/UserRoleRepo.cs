using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class UserRoleRepo : GenericRepo<UserRole>, IUserRoleRepo
    {
        private readonly AppDbContext _context;
        public UserRoleRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
