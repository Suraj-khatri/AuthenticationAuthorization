using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.EmployeeDTOs
{
    public class UpdateEmployeeDTO
    {
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public int DepartmentId { get; set; }

        public int BranchId { get; set; }

        public int? CompanyId { get; set; }

        public bool IsActive { get; set; }

        public bool IsTemporary { get; set; }

        public string? EmpStatus { get; set; }
    }
}
