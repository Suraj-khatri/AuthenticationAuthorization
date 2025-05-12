using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Department : BaseEntity
    {

        [Required]
        [StringLength(200)]
        public string DepartmentShortName { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string DepartmentName { get; set; } = null!;

        [StringLength(10)]
        public string? PhoneExtension { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Fax { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        public int EmployeeId { get; set; }

        [StringLength(50)]
        public string? MobileDepartmentHead { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string? EmailDepartmentHead { get; set; }
        public int StaticId { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; } = null!;
    }
}
