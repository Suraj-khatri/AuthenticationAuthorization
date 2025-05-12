using AuthenticationAuthorization.Application.Command.Branches;
using AuthenticationAuthorization.Application.Command.Departments;
using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.DTOs.DepartmentDTOs;
using AuthenticationAuthorization.Application.Queries.Branches;
using AuthenticationAuthorization.Application.Queries.Departments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllDepartmentsAsync()
        {
            var result = await sender.Send(new GetAllDepartmentsQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Departments = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetDepartmentById/{Id}")]
        public async Task<IActionResult> GetDepartmentByIdAsync([FromRoute] int Id)
        {
            var result = await sender.Send(new GetDepartmentByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Department = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartmentAsync([FromRoute] int id, [FromBody] UpdateDepartmentsDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateDepartmentCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 => Ok(new { Department = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartmentAsync([FromBody] AddDepartmentsDTO entity)
        {
            var result = await sender.Send(new AddDepartmentsCommand(entity), HttpContext.RequestAborted);
            return result.StatusCode switch
            {
                200 or 201 => Ok(new { Department = result }),
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
