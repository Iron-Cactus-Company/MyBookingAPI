using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Application.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.BookingActions;

public class Create
{ 
    public class Command : IRequest<Result<BookingDto>>
    {
        public Booking Booking{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<BookingDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<BookingDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var service = await _context.Service.FindAsync(request.Booking.ServiceId);
            if(service is null)
                return Result<BookingDto>.Failure(new ApplicationRequestError{ Field = "ServiceId", Type = ErrorType.NotFound });
            
            var isClientExists = await GuidHandler.IsEntityExists<Client>(request.Booking.ClientId, _context);
            if(!isClientExists)
                return Result<BookingDto>.Failure(new ApplicationRequestError{ Field = "ClientId", Type = ErrorType.NotFound});

            request.Booking.End = request.Booking.Start + service.Duration;
            _context.Booking.Add(request.Booking);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<BookingDto>.Failure(resp.error);

            var respEntity = new BookingDto();
            respEntity = _mapper.Map(request.Booking, respEntity);
            
            return Result<BookingDto>.Success(respEntity);
        }
    }
}