using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;

namespace AuthenticationAuthorization.Application.Queries.Companies;

public record GetAllCompaniesQuery : IRequest<ApiResponse<List<GetCompanyDTO>>>;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, ApiResponse<List<GetCompanyDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetCompanyDTO>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.CompanyRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetCompanyDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetCompanyDTO>>(data);
            return ApiResponse<List<GetCompanyDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetCompanyDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
