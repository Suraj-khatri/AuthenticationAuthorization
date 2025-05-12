using AuthenticationAuthorization.Application.Command.Employees;
using AuthenticationAuthorization.Application.Command.StaticDataTypes;
using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Application.Queries.StaticDataType;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticDataTypeController(ISender sender) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddStaticDataTypeAsync(AddStaticDataTypeDTO entity)
        {
            var cancellationToken = HttpContext.RequestAborted; 

            var result = await sender.Send(new AddStaticDataTypeCommand(entity), cancellationToken);

            return result.StatusCode switch
            {
                200 or 201 => Ok(new { StaticDataType = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }


        [HttpPost("Update")]
        public async Task<IActionResult> UpdateStaticDataTypeAsync(int id,UpdateStaticTypeDTO entity)
        {
            if (id != entity.Id || id <1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateGetStaticDataTypeCommand(id,entity), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { StaticDataType = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllStaticDataTypesAsync()
        {
            var result = await sender.Send(new GetAllStaticDataTypeQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { StaticDataTypes = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaticDataTypeByIdAsync(int id)
        {
            var result = await sender.Send(new GetStaticDataTypeByIdQuery(id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStaticDataTypeAsync(int id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new DeleteStaticDataTypeCommand(id), cancellationToken);

            return result.StatusCode switch
            {
                200 => Ok(result),
                404 => NotFound(result),
                400 => BadRequest(result), 
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

    }
}
