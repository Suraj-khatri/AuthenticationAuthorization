using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs
{
    public class UpdateRoleMenuPermissionDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        public int MenuPermissionId { get; set; }
    }
}
