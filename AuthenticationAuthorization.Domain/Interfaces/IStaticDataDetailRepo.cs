using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IStaticDataDetailRepo : IGenericRepo<StaticDataDetail>
    {
        Task<bool> IsExists(StaticDataDetail staticDataDetail);
    }
}
