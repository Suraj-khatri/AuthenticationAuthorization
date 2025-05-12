using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepo EmployeeRepo { get; }
        IUserRepo UserRepo { get; }
        IUserRoleRepo UserRoleRepo { get; }
        IStaticDataTypeRepo StaticDataTypeRepo { get; }
        IStaticDataDetailRepo StaticDataDetailRepo { get; }
        ICompanyRepo CompanyRepo { get; }
        IBranchRepo BranchRepo { get; } 
        IDepartmentRepo DepartmentRepo { get; }
        IMenuRepo MenuRepo { get; }
        IPermissionRepo PermissionRepo { get; }
        IMenuPermissionRepo MenuPermissionRepo { get; }
        IRoleMenuPermissionRepo RoleMenuPermissionRepo { get; }
        IRoleRepo RoleRepo { get; }
        IUserToken UserToken { get; }

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        // SaveAsync should support cancellation tokens
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }


}
