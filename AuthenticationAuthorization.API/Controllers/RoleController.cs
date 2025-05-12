using AuthenticationAuthorization.Application.Command.MenuPermissions;
using AuthenticationAuthorization.Application.Command.Roles;
using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Application.Queries.MenuPermissions;
using AuthenticationAuthorization.Application.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var result = await sender.Send(new GetAllRolesQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Roles = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetRoleById/{Id}")]
        public async Task<IActionResult> GetRoleByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetRoleByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Role = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync([FromRoute] int id, [FromBody] UpdateRoleDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateRoleCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 => Ok(new { Role = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDTO entity)
        {
            var result = await sender.Send(new AddRoleCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 or 201 => Ok(new { Role = result }),
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
