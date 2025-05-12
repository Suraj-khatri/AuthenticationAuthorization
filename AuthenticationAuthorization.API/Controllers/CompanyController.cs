using AuthenticationAuthorization.Application.Command.Companies;
using AuthenticationAuthorization.Application.Command.StaticDataTypes;
using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Application.Queries.Companies;
using AuthenticationAuthorization.Application.Queries.StaticDataType;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(ISender sender) : ControllerBase
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var result = await sender.Send(new GetAllCompaniesQuery(), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Companies = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("GetCompanyById/{Id}")]
        public async Task<IActionResult> GetCompanyByIdAsync([FromRoute]int Id)
        {
            var result = await sender.Send(new GetCompanyByIdQuery(Id), HttpContext.RequestAborted);

            return result.StatusCode switch
            {
                200 => Ok(new { Companies = result }),
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpPost("UpdateCompany/{id}")]
        public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int id, [FromBody] UpdateCompanyDTO entity)
        {
            if (id != entity.Id || id < 1)
            {
                return BadRequest(new { message = "ID in the request body does not match the ID in the URL." });
            }
            var result = await sender.Send(new UpdateCompanyCommand(entity), HttpContext.RequestAborted);

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

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] AddCompanyDTO entity)
        {
            var result = await sender.Send(new AddCompanyCommand(entity), HttpContext.RequestAborted);

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

    }
}
