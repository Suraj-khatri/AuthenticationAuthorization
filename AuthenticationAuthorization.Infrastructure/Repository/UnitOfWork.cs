using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        public IEmployeeRepo _employeeRepo;
        public IUserRepo _userRepo;
        public IUserRoleRepo _userRoleRepo;
        public IStaticDataTypeRepo _staticDataTypeRepo;
        public IStaticDataDetailRepo _staticDataDetailRepo;
        public ICompanyRepo _companyRepo;
        public IBranchRepo _branchRepo;
        public IDepartmentRepo _departmentRepo;
        public IMenuPermissionRepo _menuPermissionRepo;
        public IMenuRepo _menuRepo;
        public IRoleMenuPermissionRepo _roleMenuPermissionRepo;
        public IPermissionRepo _permissionRepo;
        public IRoleRepo _roleRepo;
        public IUserToken _userTokenRepo;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEmployeeRepo EmployeeRepo => _employeeRepo ??= new EmployeeRepo(_context);
        public IUserRepo UserRepo => _userRepo ??= new UserRepo(_context);
        public IUserRoleRepo UserRoleRepo => _userRoleRepo ??= new UserRoleRepo(_context);
        public IStaticDataTypeRepo StaticDataTypeRepo => _staticDataTypeRepo ??= new StaticDataTypeRepo(_context);
        public IStaticDataDetailRepo StaticDataDetailRepo => _staticDataDetailRepo ??= new StaticDataDetailRepo(_context);

        public ICompanyRepo CompanyRepo => _companyRepo ??= new CompanyRepo(_context);

        public IBranchRepo BranchRepo => _branchRepo ??= new BranchesRepo(_context);

        public IDepartmentRepo DepartmentRepo => _departmentRepo ??= new DepartmentRepo(_context);

        public IMenuRepo MenuRepo => _menuRepo ??= new MenuRepo(_context);

        public IPermissionRepo PermissionRepo => _permissionRepo ??= new PermissionRepo(_context);  

        public IMenuPermissionRepo MenuPermissionRepo => _menuPermissionRepo ??= new MenuPermissionRepo(_context);

        public IRoleMenuPermissionRepo RoleMenuPermissionRepo => _roleMenuPermissionRepo ??= new RoleMenuPermissionRepo(_context);
        public IRoleRepo RoleRepo => _roleRepo ??= new RoleRepo(_context);
        public IUserToken UserToken => _userTokenRepo ??= new UserTokenRepo(_context);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already in progress.");

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No transaction has been started.");

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No transaction has been started.");

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context?.Dispose();
        }
    }


}
