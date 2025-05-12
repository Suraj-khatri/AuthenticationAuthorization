using AuthenticationAuthorization.Application.DTOs.UserDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.MenuDTOs;

namespace AuthenticationAuthorization.Application.Queries.Menus;


public record GetAllMenusQuery : IRequest<ApiResponse<List<GetMenuDTO>>>;

public class GetAllMenusQueryHandler : IRequestHandler<GetAllMenusQuery, ApiResponse<List<GetMenuDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMenusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetMenuDTO>>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        try
        {
           var data = await _unitOfWork.MenuRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetMenuDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetMenuDTO>>(data);
            return ApiResponse<List<GetMenuDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetMenuDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}