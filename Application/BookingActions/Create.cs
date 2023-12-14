using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.BookingActions;

public class Create
{ 
    public class Command : IRequest<Result<Unit>>
    {
        public Booking Booking{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isServiceExists = await GuidHandler.IsEntityExists<Service>(request.Booking.ServiceId, _context);
            if(!isServiceExists)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "ServiceId", Type = ErrorType.NotFound});
            
            var isClientExists = await GuidHandler.IsEntityExists<Client>(request.Booking.ClientId, _context);
            if(!isClientExists)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "ClientId", Type = ErrorType.NotFound});
            
            _context.Booking.Add(request.Booking);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}