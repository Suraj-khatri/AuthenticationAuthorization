using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class BranchesRepo : GenericRepo<Branch>,IBranchRepo
    {
        private readonly AppDbContext _context;
        public BranchesRepo(AppDbContext context): base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> IsExists(Branch entity, CancellationToken cancellationToken = default)
        {
            return await _context.Branches
                .AnyAsync(x => x.BranchName == entity.BranchName && x.BatchCode == entity.BatchCode, cancellationToken);
        }
    }
}
