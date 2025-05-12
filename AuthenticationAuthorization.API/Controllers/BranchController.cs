using AuthenticationAuthorization.Application.Command.Branches;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using AuthenticationAuthorization.Application.Queries.Companies;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController(ISender sender) : ControllerBase
    {
        [HttpGet("get-ip")]
        public IActionResult GetClientIp()
        {
            var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            var clientIp = !string.IsNullOrEmpty(forwardedHeader)
                ? forwardedHeader.Split(',').First().Trim() // In case of multiple proxies
                : remoteIp;

            // For local dev fallback
            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return Ok(new { IP = clientIp });
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllBranchesAsync()
        {
            var result = await sender.Send(new GetAllBranchesQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Branches = result }),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetBranchById/{Id}")]
        public async Task<IActionResult> GetBrancheByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetBranchByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Branch = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("UpdateBranch")]
        public async Task<IActionResult> UpdateBranchAsync([FromRoute]int id,[FromBody] UpdateBranchesDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateBranchesCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 => Ok(new { Branch = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("AddBranch")]
        public async Task<IActionResult> AddBranchAsync([FromBody] AddBranchesDTO entity)
        {
            var result = await sender.Send(new AddBranchesCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 or 201 => Ok(new { Branches = result }),
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
