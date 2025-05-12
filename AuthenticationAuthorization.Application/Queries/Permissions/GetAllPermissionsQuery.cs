using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;

namespace AuthenticationAuthorization.Application.Queries.Permissions;

public record GetAllPermissionsQuery : IRequest<ApiResponse<List<GetPermissionDTO>>>;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, ApiResponse<List<GetPermissionDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetPermissionDTO>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.PermissionRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetPermissionDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetPermissionDTO>>(data);
            return ApiResponse<List<GetPermissionDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetPermissionDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
