using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.BranchDTOs
{
    public class UpdateBranchesDTO
    {
        public int Id { get; set; }
        public string BranchName { get; set; } = null!;

        public string BranchShortName { get; set; } = null!;

        public string? BranchCity { get; set; }

        public string BranchAddress { get; set; } = null!;

        public string BranchPhone { get; set; } = null!;

        public string? BranchEmail { get; set; }

        public string BranchDistrict { get; set; } = null!;

        public string? ContactPerson { get; set; }

        public string BatchCode { get; set; } = null!;

        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
