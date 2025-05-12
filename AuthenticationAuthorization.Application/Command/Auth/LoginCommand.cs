using AuthenticationAuthorization.Application.DTOs.AuthDTOs;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Domain.Models;
using AuthenticationAuthorization.Domain.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationAuthorization.Application.Command.Auth;

public record LoginCommand(LoginDTO loginDTO) : IRequest<ApiResponse<LoginResponseDTO>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOption _jwtOption;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IOptions<JwtOption> jwtOption)
    {
        _unitOfWork = unitOfWork;
        _jwtOption = jwtOption.Value;
    }

    public async Task<ApiResponse<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.loginDTO == null)
            {
                return ApiResponse<LoginResponseDTO>.FailureResponse("Invalid request.", 400);
            }

            var result = await ValidatePassword(request.loginDTO);
            if (!result)
            {
                return ApiResponse<LoginResponseDTO>.FailureResponse("Invalid credentials.", 401);
            }

            var loginResponse = await GetJwtTokenAsync(request.loginDTO);
            return ApiResponse<LoginResponseDTO>.SuccessResponse(loginResponse, "Login successful", 200);
        }
        catch (OperationCanceledException)
        {
            return ApiResponse<LoginResponseDTO>.FailureResponse("Operation was canceled.", 499); // Custom status code for cancellation
        }
    }

    private async Task<bool> ValidatePassword(LoginDTO loginDTO)
    {
        var user = await _unitOfWork.UserRepo.GetUserByUserNameAsync(loginDTO.UserName);
        if (user == null)
        {
            return false;
        }

        // Validate password
        if (user.UserPassword != loginDTO.Password)
        {
            return false;
        }

        return true;
    }

    private async Task<LoginResponseDTO?> GetJwtTokenAsync(LoginDTO loginDTO)
    {
        var user = await _unitOfWork.UserRepo.GetUserByUserNameAsync(loginDTO.UserName);
        var roleId =  _unitOfWork.UserRoleRepo.GetAllAsQueryable()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleId)
            .FirstOrDefault();

        var authClaims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Name", user.Name.ToString()),
            new Claim("UserName", user.UserName),
            new Claim("RoleId", roleId.ToString())
        };

        var emp = await _unitOfWork.EmployeeRepo.GetByIdAsync(Convert.ToInt32(user.Name));
        var role = await _unitOfWork.RoleRepo.GetByIdAsync(roleId);
        var branch = await _unitOfWork.BranchRepo.GetByIdAsync(emp.BranchId);

        authClaims.Add(new Claim("BranchName", branch.BranchName.ToString()));
        authClaims.Add(new Claim("DeptId", emp.DepartmentId.ToString()));
        authClaims.Add(new Claim("RoleName", role.RoleName.ToString()));

        // Generate access token.
        var jwtToken = GenerateJwtToken(authClaims);
        var accessTokenExpiration = jwtToken.ValidTo;

        // Generate refresh token.
        var refreshToken = GenerateRefreshToken();
        int refreshValidity = _jwtOption.RefreshTokenValidity;
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshValidity);

        // Set the refresh token in the database.
        await SetTokenAsync(userId: user.Id.ToString(), loginProvider: "App", name: "RefreshToken", value: refreshToken, expiry: refreshTokenExpiry);

        return new LoginResponseDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiry
        };
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
    {
        var authSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Secret));

        var token = new JwtSecurityToken(
            issuer: _jwtOption.ValidIssuer,
            audience: _jwtOption.ValidAudience,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtOption.TokenValidityInMinutes)),
            claims: authClaims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private async Task SetTokenAsync(string userId, string loginProvider, string name, string value, DateTime? expiry = null)
    {
        // Try to find existing token
        var existingToken = _unitOfWork.UserToken
            .GetAllAsQueryable()
            .Where(u => u.UserId == userId && u.LoginProvider == loginProvider && u.TokenName == name)
            .FirstOrDefault();

        if (existingToken != null)
        {
            // Update existing token
            existingToken.TokenValue = value;
            existingToken.Expiry = expiry;
            existingToken.ModifiedDate = DateTime.UtcNow; // Track when it was updated
        }
        else
        {
            var userToken = new UserToken();
            userToken.UserId = userId;
            userToken.LoginProvider = loginProvider;
            userToken.TokenName = name;
            userToken.TokenValue = value;
            userToken.Expiry = expiry;
            userToken.CreatedDate = DateTime.UtcNow; // Track when it was created
            userToken.ModifiedDate = DateTime.UtcNow; // Track when it was created

            await _unitOfWork.UserToken.InsertAsync(userToken);
        }

        await _unitOfWork.SaveAsync();
    }
}
