using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Employee : BaseEntity
    {
        [MaxLength(30)]
        public string EmpCode { get; set; } = null!;

        [MaxLength(200)]
        public string FirstName { get; set; } = null!;

        [MaxLength(200)]
        public string? MiddleName { get; set; }

        [MaxLength(200)]
        public string LastName { get; set; } = null!;

        [MaxLength(50)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? OfficialEmail { get; set; }

        public int GenderId { get; set; }
        public StaticDataDetail Gender { get; set; } = null!;

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } = null!;

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; } = null!;

        public bool IsActive { get; set; }
        [MaxLength(20)]


        public bool IsTemporary { get; set; }

        [MaxLength(50)]
        public string? EmpStatus { get; set; }
    }
}
