using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Permissions;

public record AddPermissionCommand(AddPermissionDTO permission) : IRequest<ApiResponse<GetPermissionDTO>>;

public class AddPermissionCommandHandler : IRequestHandler<AddPermissionCommand, ApiResponse<GetPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddPermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetPermissionDTO>> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
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
                return ApiResponse<GetPermissionDTO>.FailureResponse("Permission already exists.", 409);
            }

            permission.CreatedBy = 1000;
            permission.CreatedDate = DateTime.UtcNow;
            permission.ModifiedBy = 1000;
            permission.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.PermissionRepo.InsertAsync(permission);
            await _unitOfWork.SaveAsync(cancellationToken);

            var res = await _unitOfWork.PermissionRepo.GetByIdAsync(permission.Id);
            if (res == null)
            {
                return ApiResponse<GetPermissionDTO>.FailureResponse("Failed to retrieve the added permission.Check db to confirm", 500);
            }
            var result = _mapper.Map<GetPermissionDTO>(res);
            return ApiResponse<GetPermissionDTO>.SuccessResponse(result, "Permission added successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetPermissionDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}
