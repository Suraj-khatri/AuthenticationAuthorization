﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.RoleDTOs
{
    public class AddRoleDTO
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
