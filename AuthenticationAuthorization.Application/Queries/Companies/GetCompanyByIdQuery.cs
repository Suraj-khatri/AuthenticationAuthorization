using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.Companies;

public record GetCompanyByIdQuery(int Id) : IRequest<ApiResponse<GetCompanyDTO>>;
public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, ApiResponse<GetCompanyDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ApiResponse<GetCompanyDTO>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.CompanyRepo.GetByIdAsync(request.Id);
            if (data == null)
            {
                return ApiResponse<GetCompanyDTO>.FailureResponse("No data found.", 404);
            }
            var dto = _mapper.Map<GetCompanyDTO>(data);
            return ApiResponse<GetCompanyDTO>.SuccessResponse(dto, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetCompanyDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
