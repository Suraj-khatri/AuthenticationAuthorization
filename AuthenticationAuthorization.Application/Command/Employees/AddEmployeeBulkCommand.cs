using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Employees;

public record AddEmployeeBulkCommand(List<AddEmployeeDTO> employees) : IRequest<ApiResponse<List<GetEmployeeDTO>>>;

public class AddEmployeesCommandHandler : IRequestHandler<AddEmployeeBulkCommand, ApiResponse<List<GetEmployeeDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddEmployeesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetEmployeeDTO>>> Handle(AddEmployeeBulkCommand request, CancellationToken cancellationToken)
    {
        var dtoList = request.employees;

        if (dtoList == null || !dtoList.Any())
            return ApiResponse<List<GetEmployeeDTO>>.FailureResponse("No employee data provided.", 400);

        // First map to Employee entities
        var employees = dtoList.Select(dto =>
        {
            var emp = _mapper.Map<Employee>(dto);
            emp.CreatedBy = 1000;
            emp.CreatedDate = DateTime.UtcNow;
            emp.ModifiedBy = 1000;
            emp.ModifiedDate = DateTime.UtcNow;
            emp.IsActive = true;
            emp.IsTemporary = false;
            return emp;
        }).ToList();

        // Check if any of them already exist using IsExists
        var duplicateEmpCodes = new List<string>();
        foreach (var emp in employees)
        {
            if (await _unitOfWork.EmployeeRepo.IsExists(emp)) // your existing method
            {
                duplicateEmpCodes.Add(emp.EmpCode);
            }
        }

        if (duplicateEmpCodes.Any())
        {
            string duplicates = string.Join(", ", duplicateEmpCodes);
            return ApiResponse<List<GetEmployeeDTO>>.FailureResponse($"The following EmpCodes already exist: {duplicates}", 409);
        }

        // Insert Employees
        await _unitOfWork.EmployeeRepo.AddRangeAsync(employees, cancellationToken);

        // Create Users
        var users = employees.Select(e => new User
        {
            UserName = e.EmpCode,
            UserPassword = "123",
            Email = e.PhoneNumber ?? "",
            Name = e.Id.ToString(),
            IsActive = true,
            IsTemporary = false,
            CreatedBy = 1000,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = 1000,
            ModifiedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.UserRepo.AddRangeAsync(users, cancellationToken);

        // Assign Roles
        var userRoles = users.Select(u => new UserRole
        {
            UserId = u.Id,
            RoleId = 1,
            CreatedBy = 1000,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = 1000,
            ModifiedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.UserRoleRepo.AddRangeAsync(userRoles, cancellationToken);

        // Save All
        await _unitOfWork.SaveAsync(cancellationToken);

        var result = _mapper.Map<List<GetEmployeeDTO>>(employees);
        return ApiResponse<List<GetEmployeeDTO>>.SuccessResponse(result, "Employees added successfully.", 201);
    }
}

