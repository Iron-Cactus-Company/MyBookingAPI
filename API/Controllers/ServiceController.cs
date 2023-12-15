using API.Contracts.Service;
using API.Service; // Замените на нужные вам using'и
using Application.ServiceActions;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    public class ServiceController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly PermissionHelper _permissionHelper;

        public ServiceController(IMapper mapper, PermissionHelper permissionHelper)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var result = await Mediator.Send(new GetMany.Query());
            var serializedResult = _mapper.Map<IList<ServiceResponseObject>>(result.Value);

            return Ok(serializedResult);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query { Id = id });

            if (result.Value == null)
            {
                return NotFound();
            }

            var serializedResult = _mapper.Map<ServiceResponseObject>(result.Value);

            return Ok(serializedResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto createServiceDto)
        {
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), createServiceDto.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Create.Command
            {
                Service = _mapper.Map<Domain.Service>(createServiceDto)
            });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceDto updateServiceDto)
        {
            var service = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateServiceDto.Id) });
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                Service = _mapper.Map<Domain.Service>(updateServiceDto)
            });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var service = await Mediator.Send(new GetOne.Query{ Id = id });
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            var result = await Mediator.Send(new Delete.Command { Id = id });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return NoContent();
        }
    }
}
