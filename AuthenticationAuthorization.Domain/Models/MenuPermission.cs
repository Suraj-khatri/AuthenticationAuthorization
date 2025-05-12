using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class MenuPermission : BaseEntity
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
        public string Description { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; } = new List<RoleMenuPermission>();

    }
}
