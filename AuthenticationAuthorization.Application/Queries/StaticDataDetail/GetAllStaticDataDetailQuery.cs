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

public record GetAllStaticDataDetailQuery : IRequest<ApiResponse<List<GetStaticDataDetailDTO>>>;

public class GetAllStaticDataTypeQueryHandler : IRequestHandler<GetAllStaticDataDetailQuery, ApiResponse<List<GetStaticDataDetailDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllStaticDataTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetStaticDataDetailDTO>>> Handle(GetAllStaticDataDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.StaticDataDetailRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetStaticDataDetailDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetStaticDataDetailDTO>>(data);
            return ApiResponse<List<GetStaticDataDetailDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetStaticDataDetailDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
