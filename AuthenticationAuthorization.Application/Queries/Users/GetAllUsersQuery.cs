using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Application.DTOs.UserDTOs;

namespace AuthenticationAuthorization.Application.Queries.Users;


public record GetAllUsersQuery : IRequest<ApiResponse<List<GetUserDTO>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<List<GetUserDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GetUserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _unitOfWork.UserRoleRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return ApiResponse<List<GetUserDTO>>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<List<GetUserDTO>>(data);
            return ApiResponse<List<GetUserDTO>>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetUserDTO>>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
