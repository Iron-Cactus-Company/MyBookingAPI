using API.Contracts.Client;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class ClientController : BaseApiController
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            // TODO: Implement logic to get a client by ID asynchronously
            var value = $"Value {id}";
            return Ok(value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // TODO: Implement logic to get all clients asynchronously
            return Ok("List of all clients");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto createClientDto)
        {
            // TODO: Implement logic to add a client asynchronously
            var createdClientId = Guid.NewGuid().ToString();
            
            var clientResponse = new ClientResponse
            {
                Id = createdClientId,
                Name = createClientDto.Name,
                Email = createClientDto.Email
            };

            return CreatedAtAction(nameof(Get), new { id = createdClientId }, clientResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientDto updateClientDto)
        {
            var clientResponse = new ClientResponse
            {
                Id = updateClientDto.Id,
                Name = updateClientDto.Name,
                Email = updateClientDto.Email
            };

            // TODO: Implement logic to update a client asynchronously
            return Ok(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // TODO: Implement logic to delete a client asynchronously
            return NoContent();
        }
    }
}