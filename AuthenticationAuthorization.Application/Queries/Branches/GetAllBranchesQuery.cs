using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;

namespace AuthenticationAuthorization.Application.Queries.Branches;

public record GetAllBranchesQuery : IRequest<ApiResponse<List<GetBranchesDTO>>>;

public class GetAllBranchesQueryHandler : IRequestHandler<GetAllBranchesQuery, ApiResponse<List<GetBranchesDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBranchesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetBranchesDTO>>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.BranchRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetBranchesDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetBranchesDTO>>(data);
            return ApiResponse<List<GetBranchesDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetBranchesDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
