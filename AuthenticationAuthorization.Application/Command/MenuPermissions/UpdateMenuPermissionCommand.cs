using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.MenuPermissions;

public record UpdateMenuPermissionCommand(UpdateMenuPermissionDTO menuPermission) : IRequest<ApiResponse<GetMenuPermissionDTO>>;

public class UpdateMenuPermissionCommandHandler : IRequestHandler<UpdateMenuPermissionCommand, ApiResponse<GetMenuPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMenuPermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetMenuPermissionDTO>> Handle(UpdateMenuPermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.menuPermission == null)
            {
                return ApiResponse<GetMenuPermissionDTO>.FailureResponse("Invalid request.", 400);
            }

            // Map DTO to entity
            var menuPermission = _mapper.Map<MenuPermission>(request.menuPermission);

            if (await _unitOfWork.MenuPermissionRepo.IsExists(menuPermission))
            {
                return ApiResponse<GetMenuPermissionDTO>.FailureResponse("Permission already assigned for menu.", 409);
            }

            menuPermission.ModifiedBy = 1000;
            menuPermission.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.MenuPermissionRepo.UpdateAsync(menuPermission);
            await _unitOfWork.SaveAsync(cancellationToken);

            var res = await _unitOfWork.RoleRepo.GetByIdAsync(menuPermission.Id);
            if (res == null)
            {
                return ApiResponse<GetMenuPermissionDTO>.FailureResponse("Failed to retrieve the updated MenuPermission.Check db to confirm", 500);
            }
            var result = _mapper.Map<GetMenuPermissionDTO>(res);
            return ApiResponse<GetMenuPermissionDTO>.SuccessResponse(result, "MenuPermission updated successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetMenuPermissionDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}
