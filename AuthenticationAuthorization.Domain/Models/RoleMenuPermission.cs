using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class RoleMenuPermission : BaseEntity
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int MenuPermissionId { get; set; }
        public MenuPermission MenuPermission { get; set; }
    }
}
