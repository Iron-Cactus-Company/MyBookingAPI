using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BookingActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Booking>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Booking>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<Booking> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Booking>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.Booking, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<Booking>>.Success(result) : Result<List<Booking>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}