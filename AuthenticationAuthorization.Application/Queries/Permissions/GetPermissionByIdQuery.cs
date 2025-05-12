using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.Permissions;

public record GetPermissionByIdQuery(int id) : IRequest<ApiResponse<GetPermissionDTO>>;

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, ApiResponse<GetPermissionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetPermissionDTO>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.PermissionRepo.GetByIdAsync(request.id);

            if (data == null )
            {
                return ApiResponse<GetPermissionDTO>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<GetPermissionDTO>(data);
            return ApiResponse<GetPermissionDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetPermissionDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
