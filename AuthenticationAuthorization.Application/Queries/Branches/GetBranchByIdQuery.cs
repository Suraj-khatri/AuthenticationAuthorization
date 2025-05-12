using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.Branches;

public record GetBranchByIdQuery(int Id) : IRequest<ApiResponse<GetBranchesDTO>>;

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, ApiResponse<GetBranchesDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetBranchesDTO>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.BranchRepo.GetByIdAsync(request.Id);

            if (data == null)
            {
                return ApiResponse<GetBranchesDTO>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<GetBranchesDTO>(data);
            return ApiResponse<GetBranchesDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetBranchesDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}