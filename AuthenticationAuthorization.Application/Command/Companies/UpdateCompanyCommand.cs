using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuthenticationAuthorization.Application.Command.Companies;


public record UpdateCompanyCommand(UpdateCompanyDTO updateCompany) : IRequest<ApiResponse<GetCompanyDTO>>;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, ApiResponse<GetCompanyDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetCompanyDTO>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.updateCompany == null)
            {
                return ApiResponse<GetCompanyDTO>.FailureResponse("Invalid request.", 400);
            }

            var company = _mapper.Map<Company>(request.updateCompany);
            var IsExists = await _unitOfWork.CompanyRepo.IsExists(company,cancellationToken);


            if (await _unitOfWork.CompanyRepo.IsExists(company,cancellationToken))
            {
                return ApiResponse<GetCompanyDTO>.FailureResponse("Duplicate Company.", 404);
            }

            company.ModifiedBy = 1000;
            company.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.CompanyRepo.UpdateAsync(company, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            var dto = _mapper.Map<GetCompanyDTO>(company);
            return ApiResponse<GetCompanyDTO>.SuccessResponse(dto, "Updated successfully", 200);
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
