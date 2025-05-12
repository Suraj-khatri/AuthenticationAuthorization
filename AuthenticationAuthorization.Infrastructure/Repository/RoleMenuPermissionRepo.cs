using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class RoleMenuPermissionRepo : GenericRepo<RoleMenuPermission>, IRoleMenuPermissionRepo
    {
        private readonly AppDbContext _context;
        public RoleMenuPermissionRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsExists(RoleMenuPermission roleMenuPermission)
        {
            return await _context.RoleMenuPermissions.AnyAsync(x => x.RoleId == roleMenuPermission.RoleId && x.MenuPermissionId ==roleMenuPermission.MenuPermissionId);
        }
    }
}
