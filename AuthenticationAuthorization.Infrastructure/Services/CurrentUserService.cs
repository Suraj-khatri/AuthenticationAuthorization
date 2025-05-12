using AuthenticationAuthorization.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string? UserId =>
            _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

        public string? UserName =>
            _context.HttpContext?.User?.Identity?.Name;

        public int BranchId =>
            int.TryParse(_context.HttpContext?.User?.FindFirst("BranchId")?.Value, out var value) ? value : 0;

        public int RoleId =>
            int.TryParse(_context.HttpContext?.User?.FindFirst("roleId")?.Value, out var value) ? value : 0;

        public string? RoleName =>
            _context.HttpContext?.User?.FindFirst("roleName")?.Value;

        public string? BranchName =>
            _context.HttpContext?.User?.FindFirst("BranchName")?.Value;

    }

}
