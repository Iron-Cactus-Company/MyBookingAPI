using API.Attributes;
using API.Contracts.ExceptionHours;
using API.Service;
using Application.Core.Error.Enums;
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
        
        [OffsetPaginator]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var result = await Mediator.Send(new GetMany.Query());
            return HandleReadResponse<List<ExceptionHours>, List<ExceptionHoursResponseObject>>(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query { Id = id });
            return HandleReadResponse<ExceptionHours, ExceptionHoursResponseObject>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExceptionHoursDto createExceptionHoursDto)
        {
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = new Guid(createExceptionHoursDto.OpeningHoursId) });
            if (openingHours.Error.Type == ErrorType.NotFound)
                return NotFound(openingHours.Error.Field = "OpeningHoursId");
            
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Create.Command
            {
                ExceptionHours = _mapper.Map<ExceptionHours>(createExceptionHoursDto)
            });
            
            return HandleCreateResponse<ExceptionHours, ExceptionHoursResponseObject>(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExceptionHoursDto updateExceptionHoursDto)
        {
            var exceptionHours = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateExceptionHoursDto.Id) });
            if (exceptionHours.Error.Type == ErrorType.NotFound)
                return NotFound(exceptionHours.Error);
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = exceptionHours.Value.OpeningHoursId });
            if (openingHours.Error.Type == ErrorType.NotFound)
                return NotFound(openingHours.Error.Field = "OpeningHoursId");
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                ExceptionHours = _mapper.Map<ExceptionHours>(updateExceptionHoursDto)
            });

            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exceptionHours = await Mediator.Send(new GetOne.Query{ Id = id });
            if (exceptionHours.Error.Type == ErrorType.NotFound)
                return NotFound(exceptionHours.Error);
            var openingHours = await Mediator.Send(new Application.OpeningHoursActions.GetOne.Query{ Id = exceptionHours.Value.OpeningHoursId });
            if (openingHours.Error.Type == ErrorType.NotFound)
                return NotFound(openingHours.Error.Field = "OpeningHoursId");
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command { Id = id });

            return HandleDeleteResponse(result);
        }
    }
}
