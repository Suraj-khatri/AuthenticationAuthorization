using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Employees;

public record DeleteEmployeeCommand(int id) : IRequest<ApiResponse<GetEmployeeDTO>>;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ApiResponse<GetEmployeeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<GetEmployeeDTO>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var staticDataDetail = await _unitOfWork.EmployeeRepo.GetByIdAsync(request.id, cancellationToken);
            if (staticDataDetail == null)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Employee not found", 404);
            }

            await _unitOfWork.EmployeeRepo.DeleteAsync(request.id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return ApiResponse<GetEmployeeDTO>.SuccessResponse(null, "Static data detail deleted successfully", 200);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetEmployeeDTO>.FailureResponse("Operation was canceled", 400);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetEmployeeDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}
