using API.Attributes;
using API.Contracts.Booking;
using API.Enum;
using API.Service;
using API.Service.Email;
using Application.AvailableHours;
using Application.BookingActions;
using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Application.DTOs;
using Application.Enums;
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
        private readonly NotificationService _notificationService;

        public BookingController(IMapper mapper, PermissionHelper permissionHelper, NotificationService notificationService)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
            _notificationService = notificationService;
        }

        [OffsetPaginator]
        [HttpGet]
        public async Task<IActionResult> GetManyByCompanyId([FromQuery] string companyId, [FromQuery] long from, [FromQuery] long to)
        {
            Guid parsedCompanyId;
            var isValidGuid = Guid.TryParse(companyId, out parsedCompanyId);
            if (!isValidGuid)
                return BadRequest(
                    new ReadManyResponseToClient<List<BookingDto>>
                    {
                        Error = new ApplicationRequestError{ Field = "companyId", Type = ErrorType.InvalidRequest }
                    }
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
            if (!booking.IsSuccess)
                return HandleReadOneResponse<BookingDto, BookingResponseObject>(booking);
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (!service.IsSuccess)
            {
                service.Error.Field = "Booking.ServiceId";
                return HandleReadOneResponse<Domain.Service, BookingResponseObject>(service);
            }
                
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();

            var result = await Mediator.Send(new GetOne.Query { Id = id });

            return HandleReadOneResponse<BookingDto, BookingResponseObject>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto createBookingDto)
        {
            var parsedBooking = _mapper.Map<Booking>(createBookingDto);
            parsedBooking.Status = 0;
            
            var result = await Mediator.Send(new Create.Command
            {
                Booking = parsedBooking
            });

            if (!result.IsSuccess)
                return HandleCreateResponse<BookingDto, BookingResponseObject>(result);
        
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = result.Value.ServiceId });
            if (!service.IsSuccess)
            {
                service.Error.Field = "Booking.ServiceId";
                return HandleReadOneResponse<Domain.Service, BookingResponseObject>(service);
            }
            
            var company = await Mediator.Send(new Application.CompanyActions.GetOne.Query{ Id = service.Value.CompanyId });
            if (!company.IsSuccess)
            {
                service.Error.Field = "Service.CompanyId";
                return HandleReadOneResponse<Company, BookingResponseObject>(company);
            }
        
            var topic = _notificationService.GetNotificationTopic(NotificationType.BookingMade);
            var content = _notificationService.GetNotificationContent(NotificationType.BookingMade, result.Value, service.Value, company.Value);
            var receiverEmail = company.Value.Email;
        
            Console.WriteLine("--------------------");
            Console.WriteLine(receiverEmail);
            Console.WriteLine(topic);
            Console.WriteLine(content);
            Console.WriteLine("--------------------");
            
            //var message = new Message(new string[] { receiverEmail }, receiverEmail, topic, content);
            //_emailSender.SendEmailAsync(message);
            
            return HandleCreateResponse<BookingDto, BookingResponseObject>(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingDto updateBookingDto)
        {
            var booking = await Mediator.Send(new GetOne.Query{ Id = new Guid(updateBookingDto.Id) });
            if (!booking.IsSuccess)
                return HandleUpdateResponse(booking);
            
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (!service.IsSuccess)
            {
                service.Error.Field = "Booking.ServiceId";
                return HandleReadOneResponse<Domain.Service, BookingResponseObject>(service);
            }
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                Booking = _mapper.Map<Booking>(updateBookingDto)
            });
        
            if (!result.IsSuccess)
                return HandleUpdateResponse(result);
            
            var company = await Mediator.Send(new Application.CompanyActions.GetOne.Query{ Id = service.Value.CompanyId });
            if (!company.IsSuccess)
            {
                service.Error.Field = "Service.CompanyId";
                return HandleReadOneResponse<Company, BookingResponseObject>(company);
            }
            
            var notificationType = updateBookingDto.Status == (int)BookingStatusType.Accepted
                ? NotificationType.BookingAccepted
                : NotificationType.BookingDeclined;
            var topic = _notificationService.GetNotificationTopic(notificationType);
            var content = _notificationService.GetNotificationContent(notificationType, booking.Value, service.Value, company.Value);

            var client = await Mediator.Send(new Application.ClientActions.GetOne.Query{ Id = booking.Value.ClientId });
            if (client.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error.Field = "Booking.ClientId");
            var receiverEmail = client.Value.Email;
            
            Console.WriteLine("--------------------");
            Console.WriteLine(receiverEmail);
            Console.WriteLine(topic);
            Console.WriteLine(content);
            Console.WriteLine("--------------------");
            
            //var message = new Message(new string[] { receiverEmail }, receiverEmail, topic, content);
            //_emailSender.SendEmailAsync(message);
            
            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var booking = await Mediator.Send(new GetOne.Query{ Id = id });
            if (!booking.IsSuccess)
                return HandleDeleteResponse(booking);
            var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
            if (!service.IsSuccess)
            {
                service.Error.Field = "Booking.ServiceId";
                return HandleReadOneResponse<Domain.Service, BookingResponseObject>(service);
            }
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), service.Value.CompanyId))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command { Id = id });

            return HandleDeleteResponse(result);
        }
    }
}
