using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Employees;

public record AddEmployeeCommand(AddEmployeeDTO employee) : IRequest<ApiResponse<GetEmployeeDTO>>;

public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, ApiResponse<GetEmployeeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetEmployeeDTO>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check for null employee data
            if (request.employee == null)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Invalid request.", 400);
            }

            // Map DTO to entity
            var employee = _mapper.Map<Employee>(request.employee);

            // Check if the employee already exists (duplicate check)
            if (await _unitOfWork.EmployeeRepo.IsExists(employee))
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Employee already exists.", 400);
            }

            // Set default values for the new employee
            employee.IsActive = true;
            employee.IsTemporary = false;
            employee.CreatedBy = 1000; 
            employee.CreatedDate = DateTime.UtcNow;
            employee.ModifiedBy = 1000; 
            employee.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var defaultRole = await _unitOfWork.RoleRepo.GetByIdAsync(1);

            // Insert employee into DB
            await _unitOfWork.EmployeeRepo.InsertAsync(employee);

            // Insert related User entity
            var user = new User
            {
                UserName = request.employee.EmpCode,
                UserPassword = "123", // Ideally, hash this password
                Email = request.employee.PhoneNumber ?? string.Empty,
                Name = employee.Id.ToString(),
                IsActive = true,
                IsTemporary = false,
                CreatedBy = 1000, 
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = 1000, 
                ModifiedDate = DateTime.UtcNow
            };

            await _unitOfWork.UserRepo.InsertAsync(user);
            //await _unitOfWork.SaveAsync(cancellationToken);
            // Insert related UserRole entity
            var userRole = new UserRole
            {
                RoleId = defaultRole.Id, 
                UserId = user.Id,
                CreatedBy = 1000, 
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = 1000, 
                ModifiedDate = DateTime.UtcNow
            };

            await _unitOfWork.UserRoleRepo.InsertAsync(userRole,cancellationToken);
            //await _unitOfWork.SaveAsync(cancellationToken);

            // Commit all changes in one go
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var data = await _unitOfWork.EmployeeRepo.GetByIdAsync(employee.Id);
            var result = _mapper.Map<GetEmployeeDTO>(data);

            return ApiResponse<GetEmployeeDTO>.SuccessResponse(result, "Employee added successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetEmployeeDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}

