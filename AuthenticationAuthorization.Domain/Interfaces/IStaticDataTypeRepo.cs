using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IStaticDataTypeRepo : IGenericRepo<StaticDataType>
    {
        Task<bool> IsExists(StaticDataType staticDataType);
    }
}
