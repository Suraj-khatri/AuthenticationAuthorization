using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.DepartmentDTOs;

namespace AuthenticationAuthorization.Application.Queries.Departments;


public record GetAllDepartmentsQuery : IRequest<ApiResponse<List<GetDepartmentsDTO>>>;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, ApiResponse<List<GetDepartmentsDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetDepartmentsDTO>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.DepartmentRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetDepartmentsDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetDepartmentsDTO>>(data);
            return ApiResponse<List<GetDepartmentsDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetDepartmentsDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}