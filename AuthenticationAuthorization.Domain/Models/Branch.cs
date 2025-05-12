using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Branch : BaseEntity
    {


        [Required]
        [StringLength(500)]
        public string BranchName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string BranchShortName { get; set; } = null!;

        [StringLength(100)]
        public string? BranchCity { get; set; }

        [Required]
        [StringLength(1000)]
        public string BranchAddress { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string BranchPhone { get; set; } = null!;


        [StringLength(150)]
        [EmailAddress]
        public string? BranchEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string BranchDistrict { get; set; } = null!;

        [StringLength(200)]
        public string? ContactPerson { get; set; }

        [Required]
        [StringLength(15)]
        public string BatchCode { get; set; } = null!;

        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public List<Department> Departments { get; set; } 

    }
}
