using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.MenuPermissions;

public record GetRoleMenuPermissionsByIdQuery(int MenuId) : IRequest<ApiResponse<List<GetMenuPermissionDTO>>>;

public class GetPermissionsByMenuIdQueryHandler : IRequestHandler<GetRoleMenuPermissionsByIdQuery, ApiResponse<List<GetMenuPermissionDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermissionsByMenuIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetMenuPermissionDTO>>> Handle(GetRoleMenuPermissionsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var res = _unitOfWork.MenuPermissionRepo.GetAllAsQueryable();
            var data = res.Where(x => x.MenuId == request.MenuId).ToList();

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
