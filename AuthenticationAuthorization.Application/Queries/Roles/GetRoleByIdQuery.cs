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

namespace AuthenticationAuthorization.Application.Queries.Roles
{
    public record GetRoleByIdQuery(int roleId) : IRequest<ApiResponse<GetRoleDTO>>;

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ApiResponse<GetRoleDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetRoleDTO>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _unitOfWork.RoleRepo.GetByIdAsync(request.roleId);

                if (data == null)
                {
                    return ApiResponse<GetRoleDTO>.FailureResponse("No data found.", 404);
                }

                var dtoList = _mapper.Map<GetRoleDTO>(data);
                return ApiResponse<GetRoleDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
            }
            catch (Exception ex)
            {
                return ApiResponse<GetRoleDTO>.FailureResponse($"Error: {ex.Message}", 500);
            }
        }
    }
}
