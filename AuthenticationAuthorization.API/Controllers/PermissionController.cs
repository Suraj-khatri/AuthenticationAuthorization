using AuthenticationAuthorization.Application.Command.Branches;
using AuthenticationAuthorization.Application.Command.Permissions;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using AuthenticationAuthorization.Application.Queries.Permissions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPermissionsAsync()
        {
            var result = await sender.Send(new GetAllPermissionsQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Permissions = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetPermissionById/{Id}")]
        public async Task<IActionResult> GetPermissionByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetBranchByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Permission = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("UpdatePermission")]
        public async Task<IActionResult> UpdatePermissionAsync([FromRoute] int id, [FromBody] UpdatePermisionDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdatePermissionCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 => Ok(new { Permission = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermissionAsync([FromBody] AddPermissionDTO entity)
        {
            var result = await sender.Send(new AddPermissionCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 or 201 => Ok(new { Permission = result }),
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
