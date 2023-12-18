using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.OpeningHoursActions;

public class GetMany
{
    public class Query : IRequest<Result<List<OpeningHours>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<OpeningHours>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<OpeningHours> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<OpeningHours>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.OpeningHours, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<OpeningHours>>.Success(result) : Result<List<OpeningHours>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}