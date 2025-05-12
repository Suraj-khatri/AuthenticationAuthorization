using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class StaticDataDetailRepo : GenericRepo<StaticDataDetail>,IStaticDataDetailRepo
    {
        private readonly AppDbContext _context;
        public StaticDataDetailRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool>IsExists(StaticDataDetail staticDataDetail)
        {
            return await _context.StaticDataDetails.AnyAsync(x => x.DetailName == staticDataDetail.DetailName || x.DetailDesc == staticDataDetail.DetailDesc);
        }
    }
}
