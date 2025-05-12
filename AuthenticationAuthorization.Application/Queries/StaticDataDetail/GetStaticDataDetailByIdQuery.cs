using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs;

namespace AuthenticationAuthorization.Application.Queries.StaticDataDetail;

public record GetStaticDataDetailByIdQuery(int id) : IRequest<ApiResponse<GetStaticDataDetailDTO>>;
public class GetStaticDataTypeByIdQueryHandler : IRequestHandler<GetStaticDataDetailByIdQuery, ApiResponse<GetStaticDataDetailDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStaticDataTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataDetailDTO>> Handle(GetStaticDataDetailByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the ID is valid and matches the update request
            if (request.id <= 0)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Invalid ID or mismatched ID.", 400);
            }
            var data = await _unitOfWork.StaticDataDetailRepo.GetByIdAsync(request.id, cancellationToken);

            if (data == null)
            {
                return ApiResponse<GetStaticDataDetailDTO>.FailureResponse("Static data detail not found.", 404);
            }

            var dto = _mapper.Map<GetStaticDataDetailDTO>(data);
            return ApiResponse<GetStaticDataDetailDTO>.SuccessResponse(dto, "Static data detail retrieved successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetStaticDataDetailDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
