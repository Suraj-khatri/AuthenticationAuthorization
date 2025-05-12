using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.MenuPermissions;


public record AddMenuPermissionCommand(AddMenuPermissionDTO menuPermission) : IRequest<ApiResponse<GetMenuPermissionDTO>>;

public class AddMenuPermissionCommandHandler : IRequestHandler<AddMenuPermissionCommand, ApiResponse<GetMenuPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddMenuPermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetMenuPermissionDTO>> Handle(AddMenuPermissionCommand request, CancellationToken cancellationToken)
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

            menuPermission.CreatedBy = 1000;
            menuPermission.CreatedDate = DateTime.UtcNow;
            menuPermission.ModifiedBy = 1000;
            menuPermission.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.MenuPermissionRepo.InsertAsync(menuPermission);
            await _unitOfWork.SaveAsync(cancellationToken);

            var res = await _unitOfWork.MenuPermissionRepo.GetByIdAsync(menuPermission.Id);
            if (res == null)
            {
                return ApiResponse<GetMenuPermissionDTO>.FailureResponse("Failed to retrieve the added MenuPermission.Check db to confirm", 500);
            }
            var result = _mapper.Map<GetMenuPermissionDTO>(res);
            return ApiResponse<GetMenuPermissionDTO>.SuccessResponse(result, "MenuPermission added successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetMenuPermissionDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}
