using AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.StaticDataDetails;

public record AddStaticDataDetailCommand(AddStaticDataDetailDTO AddStaticDataDetail) : IRequest<ApiResponse<GetStaticDataDetailDTO>>;

public class AddStaticDataTypeCommandHandler : IRequestHandler<AddStaticDataDetailCommand, ApiResponse<GetStaticDataDetailDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddStaticDataTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataDetailDTO>> Handle(AddStaticDataDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // If the request is invalid, return a failure response
            if (request.AddStaticDataDetail == null)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Invalid request.", 400);
            }

            var staticDataDetail = _mapper.Map<StaticDataDetail>(request.AddStaticDataDetail);

            // Check if the static data type already exists, with cancellation support
            if (await _unitOfWork.StaticDataDetailRepo.IsExists(staticDataDetail))
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Duplicate static data detail.", 400);
            }

            // Set properties for the new static data type
            staticDataDetail.CreatedBy = 1000;
            staticDataDetail.ModifiedBy = 1000;
            staticDataDetail.CreatedDate = DateTime.UtcNow;
            staticDataDetail.ModifiedDate = DateTime.UtcNow;

            // Insert the new static data type with cancellation support
            await _unitOfWork.StaticDataDetailRepo.InsertAsync(staticDataDetail, cancellationToken);
            // Save the changes with cancellation support
            await _unitOfWork.SaveAsync(cancellationToken);

            // Map to DTO and return the success response
            var dto = _mapper.Map<GetStaticDataDetailDTO>(staticDataDetail);
            return ApiResponse<GetStaticDataDetailDTO>.SuccessResponse(dto, "Added successfully", 201);
        }
        catch (OperationCanceledException)
        {
            // Handle the operation cancellation (e.g., client disconnected)
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            // Handle other exceptions (e.g., database errors)
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}
