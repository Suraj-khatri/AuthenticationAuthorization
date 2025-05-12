using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.PermissionDTOs
{
    public class AddPermissionDTO
    {
        public string PermissionName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
