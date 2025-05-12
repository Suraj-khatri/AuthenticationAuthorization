using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class EmployeeRepo : GenericRepo<Employee>,IEmployeeRepo
    {
        private readonly AppDbContext _context;
        public EmployeeRepo(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<bool>IsExists(Employee employee)
        {
            return await _context.Employees.AnyAsync(x => x.EmpCode == employee.EmpCode | x.OfficialEmail == employee.OfficialEmail);
        }
    }
}
