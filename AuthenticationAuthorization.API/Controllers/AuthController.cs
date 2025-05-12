using AuthenticationAuthorization.Application.Command.Auth;
using AuthenticationAuthorization.Application.DTOs.AuthDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ISender sender) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await sender.Send(new LoginCommand(dto), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { LoginResponse = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

    }
}
