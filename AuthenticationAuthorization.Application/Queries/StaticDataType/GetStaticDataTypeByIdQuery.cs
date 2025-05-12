using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.StaticDataType;

public record GetStaticDataTypeByIdQuery(int id) : IRequest<ApiResponse<GetStaticDataTypeDTO>>;
public class GetStaticDataTypeByIdQueryHandler : IRequestHandler<GetStaticDataTypeByIdQuery, ApiResponse<GetStaticDataTypeDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStaticDataTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetStaticDataTypeDTO>> Handle(GetStaticDataTypeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the ID is valid and matches the update request
            if (request.id <= 0 )
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Invalid ID or mismatched ID.", 400);
            }
            var data = await _unitOfWork.StaticDataTypeRepo.GetByIdAsync(request.id, cancellationToken);

            if (data == null)
            {
                return ApiResponse<GetStaticDataTypeDTO>.FailureResponse("Static data type not found.", 404);
            }

            var dto = _mapper.Map<GetStaticDataTypeDTO>(data);
            return ApiResponse<GetStaticDataTypeDTO>.SuccessResponse(dto, "Static data type retrieved successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetStaticDataTypeDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}

