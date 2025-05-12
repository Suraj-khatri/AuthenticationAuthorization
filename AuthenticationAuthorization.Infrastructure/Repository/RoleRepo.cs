using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository;

public class RoleRepo : GenericRepo<Role>, IRoleRepo
{
    private readonly AppDbContext _context;
    public RoleRepo(AppDbContext context) : base(context)
    {

        _context = context;
    }
    
    public async Task<bool> IsExists(Role role)
    {
        return await _context.Roles.AnyAsync(x => x.RoleName == role.RoleName || x.Description == role.Description);
    }

}
