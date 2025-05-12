using AuthenticationAuthorization.Application.Command.Employees;
using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthenticationAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddEmployeeAsync(AddEmployeeDTO entity)
        {
            var result = await sender.Send(new AddEmployeeCommand(entity));

            return Ok(new
            {
                Employee = result,
            });
        }
    }
}
