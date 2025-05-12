using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataTypes;


public record AddStaticDataTypeCommand(AddStaticDataTypeDTO AddStaticDataType) : IRequest<ApiResponse<GetStaticDataTypeDTO>>;

public class AddStaticDataTypeCommandHandler : IRequestHandler<AddStaticDataTypeCommand, ApiResponse<GetStaticDataTypeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddStaticDataTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataTypeDTO>> Handle(AddStaticDataTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // If the request is invalid, return a failure response
            if (request.AddStaticDataType == null)  
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Invalid request.", 400);
            }

            var staticDataType = _mapper.Map<StaticDataType>(request.AddStaticDataType);

            // Check if the static data type already exists, with cancellation support
            if (await _unitOfWork.StaticDataTypeRepo.IsExists(staticDataType))
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Duplicate static data type.", 400);
            }

            // Set properties for the new static data type
            staticDataType.IsActive = true;
            staticDataType.CreatedBy = 1000;
            staticDataType.ModifiedBy = 1000;
            staticDataType.CreatedDate = DateTime.UtcNow;
            staticDataType.ModifiedDate = DateTime.UtcNow;

            // Insert the new static data type with cancellation support
            await _unitOfWork.StaticDataTypeRepo.InsertAsync(staticDataType, cancellationToken);
            // Save the changes with cancellation support
            await _unitOfWork.SaveAsync(cancellationToken);

            // Map to DTO and return the success response
            var dto = _mapper.Map<GetStaticDataTypeDTO>(staticDataType);
            return ApiResponse<GetStaticDataTypeDTO>.SuccessResponse(dto, "Added successfully", 201);
        }
        catch (OperationCanceledException)
        {
            // Handle the operation cancellation (e.g., client disconnected)
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            // Handle other exceptions (e.g., database errors)
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}
