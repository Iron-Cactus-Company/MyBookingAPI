using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ExceptionHoursActions;

public class GetMany
{
    public class Query : IRequest<Result<List<ExceptionHours>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<ExceptionHours>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<ExceptionHours> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<ExceptionHours>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.ExceptionHours, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<ExceptionHours>>.Success(result) : Result<List<ExceptionHours>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}