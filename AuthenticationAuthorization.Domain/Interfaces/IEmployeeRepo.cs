using AuthenticationAuthorization.Domain.Models;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IEmployeeRepo : IGenericRepo<Employee>
    {
        Task<bool> IsExists(Employee employee);
    }
}
