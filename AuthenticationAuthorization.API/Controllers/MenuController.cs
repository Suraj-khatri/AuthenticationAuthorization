using AuthenticationAuthorization.Application.Command.Branches;
using AuthenticationAuthorization.Application.Command.Menus;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.DTOs.MenuDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using AuthenticationAuthorization.Application.Queries.Menus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMenusAsync()
        {
            var result = await sender.Send(new GetAllMenusQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Menus = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpPost("AddMenu")]
        public async Task<IActionResult> AddMenuAsync([FromBody] AddMenuDTO entity)
        {
            var result = await sender.Send(new AddMenusCommand(entity), HttpContext.RequestAborted);
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
