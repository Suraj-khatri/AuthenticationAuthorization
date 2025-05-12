using AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.RoleMenuPermissions;

public record AddRoleMenuPermissionCommand(AddRoleMenuPermissionDTO roleMenuPermission) : IRequest<ApiResponse<GetRoleMenuPermissionDTO>>;

public class AddRoleMenuPermissionCommandHandler : IRequestHandler<AddRoleMenuPermissionCommand, ApiResponse<GetRoleMenuPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddRoleMenuPermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetRoleMenuPermissionDTO>> Handle(AddRoleMenuPermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.roleMenuPermission == null)
            {
                return ApiResponse<GetRoleMenuPermissionDTO>.FailureResponse("Invalid request.", 400);
            }

            // Map DTO to entity
            var roleMenuPermission = _mapper.Map<RoleMenuPermission>(request.roleMenuPermission);

            if (await _unitOfWork.RoleMenuPermissionRepo.IsExists(roleMenuPermission))
            {
                return ApiResponse<GetRoleMenuPermissionDTO>.FailureResponse("MenuPermission already assigned for Role.", 409);
            }

            roleMenuPermission.CreatedBy = 1000;
            roleMenuPermission.CreatedDate = DateTime.UtcNow;
            roleMenuPermission.ModifiedBy = 1000;
            roleMenuPermission.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.RoleMenuPermissionRepo.InsertAsync(roleMenuPermission);
            await _unitOfWork.SaveAsync(cancellationToken);

            var res = await _unitOfWork.RoleMenuPermissionRepo.GetByIdAsync(roleMenuPermission.Id);
            if (res == null)
            {
                return ApiResponse<GetRoleMenuPermissionDTO>.FailureResponse("Failed to retrieve the added RoleMenuPermission.Check db to confirm", 500);
            }
            var result = _mapper.Map<GetRoleMenuPermissionDTO>(res);
            return ApiResponse<GetRoleMenuPermissionDTO>.SuccessResponse(result, "RoleMenuPermission added successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetRoleMenuPermissionDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}
