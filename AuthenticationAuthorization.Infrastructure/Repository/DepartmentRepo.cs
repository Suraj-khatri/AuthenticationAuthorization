using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class DepartmentRepo : GenericRepo<Department>,IDepartmentRepo
    {
        private readonly AppDbContext _context;
        public DepartmentRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsExists(Department entity, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .AnyAsync(x => x.DepartmentName == entity.DepartmentName
                            && x.DepartmentShortName == entity.DepartmentShortName
                            && x.BranchId == entity.BranchId, cancellationToken);
        }
    }
}
