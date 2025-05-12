using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Permissions;

public record UpdatePermissionCommand(UpdatePermisionDTO permission) : IRequest<ApiResponse<GetPermissionDTO>>;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ApiResponse<GetPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetPermissionDTO>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.permission == null)
            {
                return ApiResponse<GetPermissionDTO>.FailureResponse("Invalid request.", 400);
            }

            // Map DTO to entity
            var permission = _mapper.Map<Permission>(request.permission);

            if (await _unitOfWork.PermissionRepo.IsExists(permission))
            {
                return ApiResponse<GetPermissionDTO>.FailureResponse("Permission already exists.", 400);
            }

            permission.ModifiedBy = 1000;
            permission.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.PermissionRepo.UpdateAsync(permission);
            await _unitOfWork.SaveAsync(cancellationToken);

            var result = _mapper.Map<GetPermissionDTO>(permission);
            return ApiResponse<GetPermissionDTO>.SuccessResponse(result, "Permission Updated successfully.", 204);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetPermissionDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}