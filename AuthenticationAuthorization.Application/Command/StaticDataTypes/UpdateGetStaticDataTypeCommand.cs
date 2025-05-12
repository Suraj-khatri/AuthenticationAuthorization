using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataTypes;

public record UpdateGetStaticDataTypeCommand(int id,UpdateStaticTypeDTO UpdateStaticType) : IRequest<ApiResponse<GetStaticDataTypeDTO>>;

public class UpdateGetStaticDataTypeCommandHandler : IRequestHandler<UpdateGetStaticDataTypeCommand, ApiResponse<GetStaticDataTypeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGetStaticDataTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataTypeDTO>> Handle(UpdateGetStaticDataTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Ensure the data is valid
            if (request.UpdateStaticType == null)
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Invalid request data.", 400);
            }

            // Fetch the existing static data type
            var staticDataType = await _unitOfWork.StaticDataTypeRepo.GetByIdAsync(request.id, cancellationToken);
            if (staticDataType == null)
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Static data type not found.", 404);
            }

            // Check if the static data type already exists (prevents updating to the same data)
            if (await _unitOfWork.StaticDataTypeRepo.IsExists(staticDataType))
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Static data type already exists.", 409);
            }

            // Proceed with updating the static data type
            staticDataType.IsActive = request.UpdateStaticType.IsActive;
            staticDataType.ModifiedBy = 1000;  // Assuming this is a fixed user ID for modification
            staticDataType.ModifiedDate = DateTime.UtcNow;

            // Update the entity in the database
            await _unitOfWork.StaticDataTypeRepo.UpdateAsync(staticDataType, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            // Map the updated static data type to a DTO
            var dto = _mapper.Map<GetStaticDataTypeDTO>(staticDataType);

            // Return the success response with the updated DTO
            return ApiResponse<GetStaticDataTypeDTO>.SuccessResponse(dto, "Updated successfully.", 200);
        }
        catch (Exception ex)
        {
            // Return a failure response if an unexpected error occurs
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse($"Internal error: {ex.Message}", 500);
        }
    }
}

