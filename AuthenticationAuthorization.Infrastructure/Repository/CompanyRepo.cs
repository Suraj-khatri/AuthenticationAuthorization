using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class CompanyRepo : GenericRepo<Company>, ICompanyRepo
    {
        private readonly AppDbContext _context;
        public CompanyRepo(AppDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> IsExists(Company entity, CancellationToken cancellationToken = default)
        {
            return await _context.Companies
                .AnyAsync(x => x.CompanyName == entity.CompanyName && x.CompanyPan == entity.CompanyPan, cancellationToken);
        }

    }
}
