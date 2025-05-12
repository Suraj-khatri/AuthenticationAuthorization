using AuthenticationAuthorization.Application.DTOs.DepartmentDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Departments;

public record AddDepartmentsCommand(AddDepartmentsDTO Adddept) : IRequest<ApiResponse<GetDepartmentsDTO>>;

public class AddDepartmentsCommandHandler : IRequestHandler<AddDepartmentsCommand, ApiResponse<GetDepartmentsDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDepartmentsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetDepartmentsDTO>> Handle(AddDepartmentsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Adddept == null)
            {
                return ApiResponse<GetDepartmentsDTO>.FailureResponse("Invalid request.", 400);
            }

            var dept = _mapper.Map<Department>(request.Adddept);

            if (await _unitOfWork.DepartmentRepo.IsExists(dept))
            {
                return ApiResponse<GetDepartmentsDTO>.FailureResponse("Duplicate departments data.", 404);
            }

            dept.IsActive = true;
            dept.CreatedBy = 1000;
            dept.ModifiedBy = 1000;
            dept.CreatedDate = DateTime.UtcNow;
            dept.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.DepartmentRepo.InsertAsync(dept,cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetDepartmentsDTO>(dept);
            return ApiResponse<GetDepartmentsDTO>.SuccessResponse(dto, "Added successfully", 201);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetDepartmentsDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            return ApiResponse<GetDepartmentsDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}