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

public record GetRoleByUserIdQuery(int userId) : IRequest<ApiResponse<GetUserRoleDTO>>;

public class GetRoleByUserIdQueryHandler : IRequestHandler<GetRoleByUserIdQuery, ApiResponse<GetUserRoleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRoleByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetUserRoleDTO>> Handle(GetRoleByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = _unitOfWork.UserRoleRepo.GetAllAsQueryable().Where(x => x.UserId == request.userId).FirstOrDefault();

            if (data == null)
            {
                return ApiResponse<GetUserRoleDTO>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<GetUserRoleDTO>(data);
            return ApiResponse<GetUserRoleDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetUserRoleDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
