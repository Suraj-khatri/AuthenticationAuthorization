using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.RoleDTOs;

namespace AuthenticationAuthorization.Application.Queries.Roles;


public record GetAllRolesQuery : IRequest<ApiResponse<List<GetRoleDTO>>>;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ApiResponse<List<GetRoleDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetRoleDTO>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.RoleRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetRoleDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetRoleDTO>>(data);
            return ApiResponse<List<GetRoleDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetRoleDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
