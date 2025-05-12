using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Company : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CompanyShortName { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = null!;

        [StringLength(20)]
        public string? PostBox { get; set; }

        [Required]
        [StringLength(20)]
        public string CompanyPhone { get; set; } = null!;

        [StringLength(20)]
        public string? CompanyFax { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyContactPerson { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string CompanyPan { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyEmail { get; set; } = null!;

        [StringLength(100)]
        public string? CompanyURL { get; set; }
        public List<Branch> Branches { get; set; }
    }
}
