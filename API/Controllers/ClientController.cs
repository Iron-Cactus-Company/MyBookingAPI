using API.Contracts.Client;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Create = Application.ClientActions.Create;

//Should be available only via booking request 
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
        

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto createClientDto)
        {
            var result = await Mediator.Send(new Create.Command
            {
                Client = _mapper.Map<Client>(createClientDto)
            });
            
            return HandleCreateResponse<Client, ClientResponseObject>(result);
        }
    }
}