using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BookingActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Booking>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<Booking>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Booking>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Booking.ToListAsync();
            return Result<List<Booking>>.Success(result);
        }
    }
}