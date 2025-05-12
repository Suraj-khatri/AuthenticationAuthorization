using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataTypes;

public record DeleteStaticDataTypeCommand(int id) : IRequest<ApiResponse<GetStaticDataTypeDTO>>;

public class DeleteStaticDataTypeCommandHandler : IRequestHandler<DeleteStaticDataTypeCommand, ApiResponse<GetStaticDataTypeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStaticDataTypeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<GetStaticDataTypeDTO>> Handle(DeleteStaticDataTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the StaticDataType exists before trying to delete it
            var staticDataType = await _unitOfWork.StaticDataTypeRepo.GetByIdAsync(request.id, cancellationToken);
            if (staticDataType == null)
            {
                // Return failure response if the static data type doesn't exist
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Static data type not found", 404);
            }

            // Proceed to delete the static data type
            await _unitOfWork.StaticDataTypeRepo.DeleteAsync(request.id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            // Return success response with null data if deletion was successful
            return ApiResponse<GetStaticDataTypeDTO>.SuccessResponse(null, "Static data type deleted successfully", 200);
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation scenario
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Operation was canceled", 400);
        }
        catch (Exception ex)
        {
            // Optionally log the exception or return a failure status if an unexpected error occurs
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}




