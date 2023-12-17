using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.BookingActions;

public class GetOne
{
    public class Query : IRequest<Result<Booking>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<Booking>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Booking>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.Booking.FindAsync(request.Id);

            if(result == null)
                return Result<Booking>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });

           var client = await _context.Client.FindAsync(result.ClientId);
           result.Client = client;
           
           return Result<Booking>.Success(result);
        }
    }
}