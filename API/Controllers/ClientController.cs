using API.Contracts.Client;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class ClientController : BaseApiController
    {
        
        private readonly IMapper _mapper;
        
        public ClientController(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        
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
            
            var clientResponse = _mapper.Map<ClientResponseObject>(createClientDto);

            clientResponse.Id = createdClientId;
            return CreatedAtAction(nameof(Get), new { id = createdClientId }, clientResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientDto updateClientDto)
        {
            var clientResponse = _mapper.Map<ClientResponseObject>(updateClientDto);
            clientResponse.Id = updateClientDto.Id;

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