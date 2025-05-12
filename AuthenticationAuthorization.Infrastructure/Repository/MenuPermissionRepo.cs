using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class MenuPermissionRepo : GenericRepo<MenuPermission>,IMenuPermissionRepo
    {
        private readonly AppDbContext _context;
        public MenuPermissionRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsExists(MenuPermission menuPermission)
        {
            return await _context.MenuPermissions.AnyAsync(x => x.MenuId == menuPermission.MenuId && x.PermissionId == menuPermission.PermissionId);
        }
    }
}
