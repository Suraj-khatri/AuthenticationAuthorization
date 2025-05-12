using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;

namespace AuthenticationAuthorization.Application.Queries.Employees;

public record GetEmployeeByIdQuery(int id) : IRequest<ApiResponse<GetEmployeeDTO>>;
public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, ApiResponse<GetEmployeeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetEmployeeDTO>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the ID is valid and matches the update request
            if (request.id <= 0)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Invalid ID or mismatched ID.", 400);
            }
            var data = await _unitOfWork.EmployeeRepo.GetByIdAsync(request.id, cancellationToken);

            if (data == null)
            {
                return ApiResponse<GetEmployeeDTO>.FailureResponse("Static data type not found.", 404);
            }

            var dto = _mapper.Map<GetEmployeeDTO>(data);
            return ApiResponse<GetEmployeeDTO>.SuccessResponse(dto, "Static data type retrieved successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetEmployeeDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
