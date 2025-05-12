using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class PermissionRepo : GenericRepo<Permission>,IPermissionRepo
    {
        public readonly AppDbContext _context;
        public PermissionRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsExists(Permission permission)
        {
            return await _context.Permissions.AnyAsync(x => x.PermissionName == permission.PermissionName && x.Description == permission.Description);
        }
    }
}
