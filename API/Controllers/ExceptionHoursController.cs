using API.Contracts.ExceptionHours;
using API.Contracts.OpeningHours;
using API.Service;
using Application.ExceptionHoursActions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    public class ExceptionHoursController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly PermissionHelper _permissionHelper;

        public ExceptionHoursController(IMapper mapper, PermissionHelper permissionHelper)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var result = await Mediator.Send(new GetMany.Query());
            var serializedResult = _mapper.Map<IList<OpeningHoursResponseObject>>(result.Value);

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

            var serializedResult = _mapper.Map<OpeningHoursResponseObject>(result.Value);

            return Ok(serializedResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExceptionHoursDto createExceptionHoursDto)
        {
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = createExceptionHoursDto.OpeningHoursId });
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Create.Command
            {
                ExceptionHours = _mapper.Map<ExceptionHours>(createExceptionHoursDto)
            });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExceptionHoursDto updateExceptionHoursDto)
        {
            var exceptionHours = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateExceptionHoursDto.Id) });
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = exceptionHours.Value.OpeningHoursId });
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                ExceptionHours = _mapper.Map<ExceptionHours>(updateExceptionHoursDto)
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
            var exceptionHours = await Mediator.Send(new GetOne.Query{ Id = id });
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = exceptionHours.Value.OpeningHoursId });
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
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
