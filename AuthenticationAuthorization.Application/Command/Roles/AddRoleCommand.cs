using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Roles;

public record AddRoleCommand(AddRoleDTO role) : IRequest<ApiResponse<GetRoleDTO>>;

public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, ApiResponse<GetRoleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetRoleDTO>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
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

            role.CreatedBy = 1000;
            role.CreatedDate = DateTime.UtcNow;
            role.ModifiedBy = 1000;
            role.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.RoleRepo.InsertAsync(role);
            await _unitOfWork.SaveAsync(cancellationToken);

            var res = await _unitOfWork.RoleRepo.GetByIdAsync(role.Id);
            if (res == null)
            {
                return ApiResponse<GetRoleDTO>.FailureResponse("Failed to retrieve the added role.Check db to confirm", 404);
            }
            var result = _mapper.Map<GetRoleDTO>(res);
            return ApiResponse<GetRoleDTO>.SuccessResponse(result, "Role added successfully.", 201);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return ApiResponse<GetRoleDTO>.FailureResponse($"An error occurred: {ex.Message}", 500);
        }
    }
}