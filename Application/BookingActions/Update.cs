using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.BookingActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public Booking Booking { get; init; }
    }
    
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!GuidHandler.IsGuidNull(request.Booking.ServiceId))
            {
                var isServiceExists = await GuidHandler.IsEntityExists<Service>(request.Booking.ServiceId, _context);
                if(!isServiceExists)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "ServiceId", Type = ErrorType.NotFound});
            }
            
            if (!GuidHandler.IsGuidNull(request.Booking.ClientId))
            {
                var isClientExists = await GuidHandler.IsEntityExists<Client>(request.Booking.ClientId, _context);
                if(!isClientExists)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "ClientId", Type = ErrorType.NotFound});
            }
            
            var itemToUpdate = await _context.Booking.FindAsync(request.Booking.Id);
            if (itemToUpdate == null)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Id", Type = ErrorType.NotFound});

            if (request.Booking.Start == 0)
                request.Booking.Start = itemToUpdate.Start;
            
            if (request.Booking.End == 0)
                request.Booking.End = itemToUpdate.End;
            
            _mapper.Map(request.Booking, itemToUpdate);
            
            var resp = ResponseDeterminer.DetermineUpdateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}