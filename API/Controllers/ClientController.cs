using API.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController] 
public class ClientController : BaseApiController 
{
        [HttpPost]
        public IActionResult Create([FromBody] CreateClientDto createClientDto)
        {
            return Ok("Client created successfully"); 
        }
}
