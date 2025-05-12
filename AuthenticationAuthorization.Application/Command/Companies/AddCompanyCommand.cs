using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Companies;

public record AddCompanyCommand(AddCompanyDTO AddCompany  ) : IRequest<ApiResponse<GetCompanyDTO>>;

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, ApiResponse<GetCompanyDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetCompanyDTO>> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AddCompany == null)
            {
                return ApiResponse<GetCompanyDTO>.FailureResponse("Invalid request.", 400);
            }

            var company = _mapper.Map<Company>(request.AddCompany);

            if (await _unitOfWork.CompanyRepo.IsExists(company))
            {
                return ApiResponse<GetCompanyDTO>.FailureResponse("Duplicate company data.", 404);
            }

            company.IsActive = true;
            company.CreatedBy = 1000;
            company.ModifiedBy = 1000;
            company.CreatedDate = DateTime.UtcNow;
            company.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.CompanyRepo.InsertAsync(company, cancellationToken);
           
            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetCompanyDTO>(company);
            return ApiResponse<GetCompanyDTO>.SuccessResponse(dto, "Added successfully", 201);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<GetCompanyDTO>.FailureResponse("Operation was canceled.", 499); // Optional: Custom status code for cancellation
        }
        catch (Exception ex)
        {
            return ApiResponse<GetCompanyDTO>.FailureResponse($"Internal server error: {ex.Message}", 500);
        }
    }
}
