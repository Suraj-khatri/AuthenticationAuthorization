using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs
{
    public class GetRoleMenuPermissionDTO
    {
        public int Id { get; set; }
        public string Role { get; set; }


        public int MenuPermissionId { get; set; }
    }
}
