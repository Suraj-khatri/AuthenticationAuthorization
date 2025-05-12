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

namespace AuthenticationAuthorization.Application.Queries.Users;

public record GetUserByUserNameQuery(string UserName) : IRequest<ApiResponse<GetUserDTO>>;

public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, ApiResponse<GetUserDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByUserNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetUserDTO>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var res =  _unitOfWork.UserRepo.GetAllAsQueryable();
            var data = res.Where(x => x.UserName == request.UserName).FirstOrDefault();

            if (data == null)
            {
                return ApiResponse<GetUserDTO>.FailureResponse("No data found.", 404);
            }

            var dtoList = _mapper.Map<GetUserDTO>(data);
            return ApiResponse<GetUserDTO>.SuccessResponse(dtoList, "Data fetched successfully.", 200);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetUserDTO>.FailureResponse($"Error: {ex.Message}", 500);
        }
    }
}
