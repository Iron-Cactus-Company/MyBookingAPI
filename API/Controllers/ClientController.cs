using API.Contracts.Client;
using Application.ClientActions;
using Application.Core;
using AutoMapper;
using Domain;
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
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetMany.Query());
            var serializedResult = _mapper.Map<IList<ClientResponseObject>>(result.Value);
            
            return Ok(serializedResult);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query{ Id = id});
            
            if (result.Value == null)
            {
                return NotFound();
            }
            var serializedResult = _mapper.Map<ClientResponseObject>(result.Value);
            
            return Ok(serializedResult);
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto createClientDto)
        {
            // todo result returns empty value 
            var result = await Mediator.Send(new Create.Command
            {
                Client = _mapper.Map<Client>(createClientDto)
            });
            
            
            if (!result.IsSuccess)
            {
                return Conflict();
            }

          
            return Ok(result);

            // var serializedResult = _mapper.Map<ClientResponseObject>(result.Value);
            //
            // return CreatedAtAction(nameof(Get), new { id = serializedResult.Id }, serializedResult);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientDto updateClientDto)
        {
            
            // todo result returns empty value ; Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred 
            var result = await Mediator.Send(new Create.Command
            {
                Client = _mapper.Map<Client>(updateClientDto)
            });
            
            if (!result.IsSuccess)
            {
                return Conflict();
            }
            return Ok(result);
            // var serializedResult = _mapper.Map<ClientResponseObject>(result);
            // return Ok(serializedResult);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //todo fix internal error 500 when notfound
            var result = await Mediator.Send(new Delete.Command{ Id = id});
            if (!result.IsSuccess)
            {
                return Conflict();
            }
            return NoContent();
        }
    }
}