using AuthenticationAuthorization.Application.Command.MenuPermissions;
using AuthenticationAuthorization.Application.Command.RoleMenuPermissions;
using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs;
using AuthenticationAuthorization.Application.Queries.MenuPermissions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenuPermissionController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAllRoleMenuPermissions")]
        public async Task<IActionResult> GetAllRoleMenuPermissionsAsync()
        {
            var result = await sender.Send(new GetAllRoleMenuPermissionsQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { RoleMenuPermissions = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetRoleMenuPermissionById/{Id}")]
        public async Task<IActionResult> GetRoleMenuPermissionByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetRoleMenuPermissionsByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { RoleMenuPermission = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("UpdateRoleMenuPermission")]
        public async Task<IActionResult> UpdateRoleMenuPermissionAsync([FromRoute] int id, [FromBody] UpdateRoleMenuPermissionDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateRoleMenuPermissionCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 => Ok(new { MenuPermission = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("AddRoleMenuPermission")]
        public async Task<IActionResult> AddRoleMenuPermissionAsync([FromBody] AddRoleMenuPermissionDTO entity)
        {
            var result = await sender.Send(request: new AddRoleMenuPermissionCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 or 201 => Ok(new { MenuPermission = result }),
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
