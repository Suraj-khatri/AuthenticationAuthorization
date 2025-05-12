using AuthenticationAuthorization.Application.DTOs.MenuDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Menus;


public record AddMenusCommand(AddMenuDTO AddMenu) : IRequest<ApiResponse<GetMenuDTO>>;

public class AddMenusCommandHandler : IRequestHandler<AddMenusCommand, ApiResponse<GetMenuDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddMenusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetMenuDTO>> Handle(AddMenusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AddMenu == null)
            {
                return ApiResponse<GetMenuDTO>.FailureResponse("Invalid request.", 400);
            }

            var menu = _mapper.Map<Menu>(request.AddMenu);

            //if (await _unitOfWork.DepartmentRepo.IsExists(menu))
            //{
            //    return ApiResponse<GetMenuDTO>.FailureResponse("Duplicate departments data.", 404);
            //}

            //menu.IsActive = true;
            menu.CreatedBy = 1000;
            menu.ModifiedBy = 1000;
            menu.CreatedDate = DateTime.UtcNow;
            menu.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.MenuRepo.InsertAsync(menu, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetMenuDTO>(menu);
            return ApiResponse<GetMenuDTO>.SuccessResponse(dto, "Added successfully", 201);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetMenuDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            return ApiResponse<GetMenuDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}