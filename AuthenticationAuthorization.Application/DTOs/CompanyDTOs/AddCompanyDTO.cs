using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.CompanyDTOs
{
    public class AddCompanyDTO
    {
        public string CompanyName { get; set; } = null!;

        public string CompanyShortName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? PostBox { get; set; }

        public string CompanyPhone { get; set; } = null!;

        public string? CompanyFax { get; set; }

        public string CompanyContactPerson { get; set; } = null!;

        public string CompanyPan { get; set; } = null!;

        public bool IsActive { get; set; }

        public string CompanyEmail { get; set; } = null!;

        public string? CompanyURL { get; set; }
    }
}
