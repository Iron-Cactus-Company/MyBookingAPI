﻿using API.Contracts.Service;
using API.Service;
using Application.Core.Error.Enums;
using Application.ServiceActions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var result = await Mediator.Send(new GetMany.Query());
            return HandleReadResponse<List<Domain.Service>, List<ServiceResponseObject>>(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query { Id = id });
            return HandleReadResponse<Domain.Service, ServiceResponseObject>(result);
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

            return HandleCreateResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceDto updateServiceDto)
        {
            var service = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateServiceDto.Id) });
            if (service.Error.Type == ErrorType.NotFound)
                return NotFound(service.Error);
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                Service = _mapper.Map<Domain.Service>(updateServiceDto)
            });

            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var service = await Mediator.Send(new GetOne.Query{ Id = id });
            if (service.Error.Type == ErrorType.NotFound)
                return NotFound(service.Error);
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command { Id = id });
            return HandleDeleteResponse(result);
        }
    }
}
