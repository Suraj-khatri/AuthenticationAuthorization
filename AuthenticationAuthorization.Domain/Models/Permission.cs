using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Permission : BaseEntity
    {
        [MaxLength(100)]
        public string PermissionName { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
        public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();
    }
}
