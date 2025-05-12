using AuthenticationAuthorization.Application.Command.Branches;
using AuthenticationAuthorization.Application.Command.MenuPermissions;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using AuthenticationAuthorization.Application.Queries.MenuPermissions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuPermissionController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAllMenuPermissions")]
        public async Task<IActionResult> GetAllMenuPermissionsAsync()
        {
            var result = await sender.Send(new GetAllRoleMenuPermissionsQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { MenuPermissions = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetMenuPermissionById/{Id}")]
        public async Task<IActionResult> GetMenuPermissionByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetRoleMenuPermissionsByIdQuery(Id), HttpContext.RequestAborted);

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
        [HttpPost("UpdateMenuPermission")]
        public async Task<IActionResult> UpdateMenuPermissionAsync([FromRoute] int id, [FromBody] UpdateMenuPermissionDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateMenuPermissionCommand(entity), HttpContext.RequestAborted);
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
        [HttpPost("AddMenuPermission")]
        public async Task<IActionResult> AddMenuPermissionAsync([FromBody] AddMenuPermissionDTO entity)
        {
            var result = await sender.Send(new AddMenuPermissionCommand(entity), HttpContext.RequestAborted);
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
