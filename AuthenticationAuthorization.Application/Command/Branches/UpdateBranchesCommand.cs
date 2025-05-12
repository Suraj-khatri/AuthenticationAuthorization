using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Branches;


public record UpdateBranchesCommand(UpdateBranchesDTO Addbranch) : IRequest<ApiResponse<GetBranchesDTO>>;

public class UpdateBranchesCommandHandler : IRequestHandler<UpdateBranchesCommand, ApiResponse<GetBranchesDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBranchesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetBranchesDTO>> Handle(UpdateBranchesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Addbranch == null)
            {
                return ApiResponse<GetBranchesDTO>.FailureResponse("Invalid request.", 400);
            }

            var branch = _mapper.Map<Branch>(request.Addbranch);

            if (await _unitOfWork.BranchRepo.IsExists(branch))
            {
                return ApiResponse<GetBranchesDTO>.FailureResponse("Duplicate branch data.", 404);
            }

            branch.IsActive = true;
            branch.ModifiedBy = 1000;
            branch.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.BranchRepo.UpdateAsync(branch, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetBranchesDTO>(branch);
            return ApiResponse<GetBranchesDTO>.SuccessResponse(dto, "updated successfully", 200);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetBranchesDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            return ApiResponse<GetBranchesDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}