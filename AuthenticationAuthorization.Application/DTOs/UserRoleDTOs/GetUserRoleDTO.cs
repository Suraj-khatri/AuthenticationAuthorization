using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.UserRoleDTOs
{
    public class GetUserRoleDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
