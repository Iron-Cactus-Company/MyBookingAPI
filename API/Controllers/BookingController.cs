﻿using API.Contracts.Booking;
using API.Service;
using Application.BookingActions;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BookingController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly PermissionHelper _permissionHelper;

        public BookingController(IMapper mapper, PermissionHelper permissionHelper)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetMany()
        // {
        //     var result = await Mediator.Send(new GetMany.Query());
        //     return HandleReadResponse<List<Booking>, List<BookingResponseObject>>(result);
        // }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await Mediator.Send(new GetOne.Query{ Id = id });
            if (booking.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error);
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (service.Error.Type == ErrorType.NotFound)
                return NotFound(service.Error.Field = "ServiceId");
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();

            var result = await Mediator.Send(new GetOne.Query { Id = id });

            return HandleReadResponse<Booking, BookingResponseObject>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto createBookingDto)
        {
            var result = await Mediator.Send(new Create.Command
            {
                Booking = _mapper.Map<Booking>(createBookingDto)
            });

            return HandleCreateResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingDto updateBookingDto)
        {
            var booking = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateBookingDto.Id) });
            if (booking.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error);
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (service.Error.Type == ErrorType.NotFound)
                return NotFound(service.Error.Field = "ServiceId");
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                Booking = _mapper.Map<Booking>(updateBookingDto)
            });

            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var booking = await Mediator.Send(new GetOne.Query{ Id = id });
            if (booking.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error);
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (service.Error.Type == ErrorType.NotFound)
                return NotFound(service.Error.Field = "ServiceId");
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command { Id = id });

            return HandleDeleteResponse(result);
        }
    }
}
