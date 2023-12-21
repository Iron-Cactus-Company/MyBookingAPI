using API.Attributes;
using API.Contracts.Booking;
using API.Service;
using Application.AvailableHours;
using Application.BookingActions;
using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Application.DTOs;
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

        [OffsetPaginator]
        [HttpGet]
        public async Task<IActionResult> GetManyByCompanyId([FromQuery] string companyId, [FromQuery] long from, [FromQuery] long to)
        {
            Guid parsedCompanyId;
            var isValidGuid = Guid.TryParse(companyId, out parsedCompanyId);
            if (!isValidGuid)
                return BadRequest(
                    new ReadManyResponseToClient<List<BookingDto>>{ Error = new ApplicationRequestError{ Field = "companyId", Type = ErrorType.InvalidRequest } }
                    );
            
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), parsedCompanyId))
                return Unauthorized();
            
            var limit = (int)HttpContext.Items["limit"];
            var page = (int)HttpContext.Items["page"];
            var options = new ReadOptions { Limit = limit, PageNumber = page };

            Result<List<BookingDto>> result;
            if (from == default && to == default)
            {
                result = await Mediator.Send(new FindByCompanyId.Query
                {
                    CompanyId = parsedCompanyId,
                    Options = options
                });
            }
            else
            {
                result = await Mediator.Send(new FindByInterval.Query
                {
                    CompanyId = parsedCompanyId, 
                    Interval = new Interval(from, to),
                    Options = options
                });
            }
            
            return HandleReadOneResponse<List<BookingDto>, List<BookingResponseObject>>(result);
        }

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

            return HandleReadOneResponse<BookingDto, BookingResponseObject>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto createBookingDto)
        {
            var result = await Mediator.Send(new Create.Command
            {
                Booking = _mapper.Map<Booking>(createBookingDto)
            });

            return HandleCreateResponse<BookingDto, BookingResponseObject>(result);
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
