﻿using API.Attributes;
using API.Contracts.OpeningHours;
using API.Service;
using Application.Core.Error.Enums;
using Application.OpeningHoursActions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    public class OpeningHoursController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly PermissionHelper _permissionHelper;

        public OpeningHoursController(IMapper mapper, PermissionHelper permissionHelper)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }

        [OffsetPaginator]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMany([FromQuery] string companyId)
        {
            if (companyId.IsNullOrEmpty())
            {
                var result = await Mediator.Send(new GetMany.Query());
                return HandleReadManyResponse<List<OpeningHours>, List<OpeningHoursResponseObject>>(result);
            }
            
            var resp = await Mediator.Send(new FindOneByCompanyId.Query{CompanyId = new Guid(companyId)});
            return HandleReadOneResponse<OpeningHours, OpeningHoursResponseObject>(resp);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query { Id = id });
            return HandleReadOneResponse<OpeningHours, OpeningHoursResponseObject>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOpeningHoursDto createOpeningHoursDto)
        {
            if (!await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), createOpeningHoursDto.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Create.Command
            {
                OpeningHours = _mapper.Map<OpeningHours>(createOpeningHoursDto)
            });
            
            return HandleCreateResponse<OpeningHours, OpeningHoursResponseObject>(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOpeningHoursDto updateOpeningHoursDto)
        {
            var openingHours = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateOpeningHoursDto.Id) });
            if (!openingHours.IsSuccess)
                HandleUpdateResponse(openingHours);
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                OpeningHours = _mapper.Map<OpeningHours>(updateOpeningHoursDto)
            });

            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var openingHours = await Mediator.Send(new GetOne.Query{ Id = id });
            if (!openingHours.IsSuccess)
                return HandleDeleteResponse(openingHours);
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), openingHours.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command { Id = id });
            return HandleDeleteResponse(result);
        }
    }
}
