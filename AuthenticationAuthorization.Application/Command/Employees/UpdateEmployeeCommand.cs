using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Employees;


public record UpdateEmployeeCommand(int id, UpdateEmployeeDTO UpdateEmployee) : IRequest<ApiResponse<GetEmployeeDTO>>;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ApiResponse<GetEmployeeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetEmployeeDTO>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UpdateEmployee == null)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Invalid request data.", 400);
            }

            var employee = await _unitOfWork.EmployeeRepo.GetByIdAsync(request.id, cancellationToken);
            if (employee == null)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Employee not found.", 404);
            }

            if (await _unitOfWork.EmployeeRepo.IsExists(employee))
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Employee already exists.", 409);
            }

            employee.ModifiedBy = 1000;  
            employee.ModifiedDate = DateTime.UtcNow;
            employee.IsActive = request.UpdateEmployee.IsActive;
            employee.IsTemporary = request.UpdateEmployee.IsTemporary;
            employee.FirstName = request.UpdateEmployee.FirstName;
            employee.LastName = request.UpdateEmployee.LastName;
            employee.PhoneNumber = request.UpdateEmployee.PhoneNumber;
            employee.DepartmentId = request.UpdateEmployee.DepartmentId;



            await _unitOfWork.EmployeeRepo.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetEmployeeDTO>(employee);

            return ApiResponse<GetEmployeeDTO>.SuccessResponse(dto, "Updated successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetEmployeeDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}
