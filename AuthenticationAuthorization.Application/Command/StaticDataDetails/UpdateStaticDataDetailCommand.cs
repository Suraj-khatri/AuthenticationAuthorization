using AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataDetails;

public record UpdateStaticDataDetailCommand(int id, UpdateStaticDataDetailDTO  UpdateStaticDataDetail) : IRequest<ApiResponse<GetStaticDataDetailDTO>>;

public class UpdateGetStaticDataDetailCommandHandler : IRequestHandler<UpdateStaticDataDetailCommand, ApiResponse<GetStaticDataDetailDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGetStaticDataDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataDetailDTO>> Handle(UpdateStaticDataDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Ensure the data is valid
            if (request.UpdateStaticDataDetail == null)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Invalid request data.", 400);
            }

            // Fetch the existing static data type
            var staticDataDetail = await _unitOfWork.StaticDataDetailRepo.GetByIdAsync(request.id, cancellationToken);
            if (staticDataDetail == null)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Static data detail not found.", 404);
            }

            // Check if the static data type already exists (prevents updating to the same data)
            if (await _unitOfWork.StaticDataDetailRepo.IsExists(staticDataDetail))
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Static data detail already exists.", 409);
            }

            // Proceed with updating the static data type
            staticDataDetail.ModifiedBy = 1000;  // Assuming this is a fixed user ID for modification
            staticDataDetail.ModifiedDate = DateTime.UtcNow;

            // Update the entity in the database
            await _unitOfWork.StaticDataDetailRepo.UpdateAsync(staticDataDetail, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            // Map the updated static data type to a DTO
            var dto = _mapper.Map<GetStaticDataDetailDTO>(staticDataDetail);

            // Return the success response with the updated DTO
            return ApiResponse<GetStaticDataDetailDTO>.SuccessResponse(dto, "Updated successfully.", 200);
        }
        catch (Exception ex)
        {
            // Return a failure response if an unexpected error occurs
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}
