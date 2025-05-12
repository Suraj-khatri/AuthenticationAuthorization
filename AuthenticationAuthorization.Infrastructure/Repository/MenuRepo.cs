using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class MenuRepo : GenericRepo<Menu>,IMenuRepo
    {
        public MenuRepo(AppDbContext context) : base(context)
        {
            
        }
    }
}
