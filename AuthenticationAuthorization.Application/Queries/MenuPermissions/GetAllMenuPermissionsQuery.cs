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
using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using System.Xml.Serialization;

namespace AuthenticationAuthorization.Application.Queries.MenuPermissions;


public record GetAllRoleMenuPermissionsQuery : IRequest<ApiResponse<List<GetMenuPermissionDTO>>>;

public class GetAllMenuPermissionsQueryHandler : IRequestHandler<GetAllRoleMenuPermissionsQuery, ApiResponse<List<GetMenuPermissionDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMenuPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetMenuPermissionDTO>>> Handle(GetAllRoleMenuPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.MenuPermissionRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetMenuPermissionDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetMenuPermissionDTO>>(data);
            return ApiResponse<List<GetMenuPermissionDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetMenuPermissionDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
