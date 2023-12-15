﻿using API.Contracts.OpeningHours;
using Application.OpeningHoursActions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain;

namespace API.Controllers
{
    [ApiController]
    public class OpeningHoursController : BaseApiController
    {
        private readonly IMapper _mapper;

        public OpeningHoursController(IMapper mapper)
        {
            _mapper = mapper;
        }

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
        public async Task<IActionResult> Create([FromBody] CreateOpeningHoursDto createOpeningHoursDto)
        {
            var result = await Mediator.Send(new Create.Command
            {
                OpeningHours = _mapper.Map<OpeningHours>(createOpeningHoursDto)
            });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOpeningHoursDto updateOpeningHoursDto)
        {
            var result = await Mediator.Send(new Update.Command
            {
                OpeningHours = _mapper.Map<OpeningHours>(updateOpeningHoursDto)
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
            var result = await Mediator.Send(new Delete.Command { Id = id });

            if (!result.IsSuccess)
            {
                return Conflict();
            }

            return NoContent();
        }
    }
}