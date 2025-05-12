using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Roles;

public record UpdateRoleCommand(UpdateRoleDTO role) : IRequest<ApiResponse<GetRoleDTO>>;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ApiResponse<GetRoleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetRoleDTO>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.role == null)
            {
                return ApiResponse<GetRoleDTO>.FailureResponse("Invalid request.", 400);
            }

            // Map DTO to entity
            var role = _mapper.Map<Role>(request.role);

            if (await _unitOfWork.RoleRepo.IsExists(role))
            {
                return ApiResponse<GetRoleDTO>.FailureResponse("Role already exists.", 400);
            }

            role.ModifiedBy = 1000;
            role.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.RoleRepo.UpdateAsync(role);
            await _unitOfWork.SaveAsync(cancellationToken);

            var result = _mapper.Map<GetRoleDTO>(role);
            return ApiResponse<GetRoleDTO>.SuccessResponse(result, "Role Updated successfully.", 204);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetRoleDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}
