using AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataDetails;

public record DeleteStaticDataDetailCommand(int id) : IRequest<ApiResponse<GetStaticDataDetailDTO>>;

public class DeleteStaticDataDeleteCommandHandler : IRequestHandler<DeleteStaticDataDetailCommand, ApiResponse<GetStaticDataDetailDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStaticDataDeleteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<GetStaticDataDetailDTO>> Handle(DeleteStaticDataDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var staticDataDetail = await _unitOfWork.StaticDataDetailRepo.GetByIdAsync(request.id, cancellationToken);
            if (staticDataDetail == null)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Static data detail not found", 404);
            }

            await _unitOfWork.StaticDataDetailRepo.DeleteAsync(request.id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return ApiResponse<GetStaticDataDetailDTO>.SuccessResponse(null, "Static data detail deleted successfully", 200);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Operation was canceled", 400);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}
