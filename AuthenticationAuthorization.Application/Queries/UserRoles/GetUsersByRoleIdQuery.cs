using AuthenticationAuthorization.Application.DTOs.UserRoleDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.UserRoles;


public record GetUsersByRoleIdQuery(int roleId) : IRequest<ApiResponse<List<GetUserRoleDTO>>>;

public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, ApiResponse<List<GetUserRoleDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUsersByRoleIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetUserRoleDTO>>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data =  _unitOfWork.UserRoleRepo.GetAllAsQueryable().Where(x => x.RoleId == request.roleId).ToList();
            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetUserRoleDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetUserRoleDTO>>(data);
            return ApiResponse<List<GetUserRoleDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetUserRoleDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
