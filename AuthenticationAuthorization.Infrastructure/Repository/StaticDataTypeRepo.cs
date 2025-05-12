using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class StaticDataTypeRepo : GenericRepo<StaticDataType>,IStaticDataTypeRepo
    {
        private readonly AppDbContext _context;
        public StaticDataTypeRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool>IsExists(StaticDataType staticDataType)
        {
            return await _context.StaticDataType.AnyAsync(x => x.TypeTitle == staticDataType.TypeTitle);
        }
    }
}
