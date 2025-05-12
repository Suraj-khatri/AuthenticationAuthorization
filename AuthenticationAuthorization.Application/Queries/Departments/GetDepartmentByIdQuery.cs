using AuthenticationAuthorization.Application.DTOs.DepartmentDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.Departments;

public record GetDepartmentByIdQuery(int Id) : IRequest<ApiResponse<GetDepartmentsDTO>>;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, ApiResponse<GetDepartmentsDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetDepartmentsDTO>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.DepartmentRepo.GetByIdAsync(request.Id);

            if (data == null)
            {
                return ApiResponse<GetDepartmentsDTO>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<GetDepartmentsDTO>(data);
            return ApiResponse<GetDepartmentsDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetDepartmentsDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
