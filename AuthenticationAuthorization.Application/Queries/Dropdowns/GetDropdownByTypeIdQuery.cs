using AuthenticationAuthorization.Application.DTOs.DropdownDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Queries.Dropdowns;

public record GetDropdownByTypeIdQuery(int TypeId) : IRequest<ApiResponse<List<DropdownDTO>>>;
public class GetDropdownByTypeIdQueryHandler : IRequestHandler<GetDropdownByTypeIdQuery, ApiResponse<List<DropdownDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDropdownByTypeIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<DropdownDTO>>> Handle(GetDropdownByTypeIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = _unitOfWork.StaticDataDetailRepo
                                  .GetAllAsQueryable()
                                  .Where(x => x.TypeId == request.TypeId)
                                  .ToList();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<DropdownDTO>>.FailureResponse("No dropdown data found.", 404);
            }

            var result = data.Select(item => new DropdownDTO
            {
                Id = item.Id,
                Title = item.DetailName
            }).ToList();

            return ApiResponse<List<DropdownDTO>>.SuccessResponse(result, "Dropdown data retrieved successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<DropdownDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
