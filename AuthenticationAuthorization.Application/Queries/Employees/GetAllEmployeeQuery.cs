using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Queries.Employees;

public record GetAllEmployeeQuery : IRequest<ApiResponse<List<GetEmployeeDTO>>>;

public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeeQuery, ApiResponse<List<GetEmployeeDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetEmployeeDTO>>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.EmployeeRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetEmployeeDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetEmployeeDTO>>(data);
            return ApiResponse<List<GetEmployeeDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetEmployeeDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
