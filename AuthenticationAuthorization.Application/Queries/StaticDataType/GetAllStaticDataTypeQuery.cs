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

public record GetAllStaticDataTypeQuery : IRequest<ApiResponse<List<GetStaticDataTypeDTO>>>;

public class GetAllStaticDataTypeQueryHandler : IRequestHandler<GetAllStaticDataTypeQuery, ApiResponse<List<GetStaticDataTypeDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllStaticDataTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetStaticDataTypeDTO>>> Handle(GetAllStaticDataTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.StaticDataTypeRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetStaticDataTypeDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetStaticDataTypeDTO>>(data);
            return ApiResponse<List<GetStaticDataTypeDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetStaticDataTypeDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}

