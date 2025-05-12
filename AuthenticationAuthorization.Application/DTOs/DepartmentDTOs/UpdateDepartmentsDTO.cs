using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.DepartmentDTOs
{
    public class UpdateDepartmentsDTO
    {
        public int Id { get; set; }
        public string DepartmentShortName { get; set; } = null!;

        public string DepartmentName { get; set; } = null!;

        public string? PhoneExtension { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public int DepartmentHeadId { get; set; }

        public string? MobileDepartmentHead { get; set; }

        public string? EmailDepartmentHead { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
    }
}
