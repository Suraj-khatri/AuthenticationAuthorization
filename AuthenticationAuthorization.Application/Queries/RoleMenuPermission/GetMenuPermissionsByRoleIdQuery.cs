using AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.RoleMenuPermission;



public record GetMenuPermissionsByRoleIdQuery(int roleId) : IRequest<ApiResponse<List<GetRoleMenuPermissionDTO>>>;

public class GetMenuPermissionsByRoleIdQueryHandler : IRequestHandler<GetMenuPermissionsByRoleIdQuery, ApiResponse<List<GetRoleMenuPermissionDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMenuPermissionsByRoleIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetRoleMenuPermissionDTO>>> Handle(GetMenuPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data =  _unitOfWork.RoleMenuPermissionRepo.GetAllAsQueryable().Where(x=>x.RoleId == request.roleId).ToList();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetRoleMenuPermissionDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetRoleMenuPermissionDTO>>(data);
            return ApiResponse<List<GetRoleMenuPermissionDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetRoleMenuPermissionDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}