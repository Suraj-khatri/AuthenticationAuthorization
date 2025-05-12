using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        int BranchId { get; }
        string? BranchName { get; }
        int RoleId { get; }
        string? RoleName { get; }
    }
}
