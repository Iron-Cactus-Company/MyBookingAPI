using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ServiceActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Service>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Service>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<Service> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Service>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.Service, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<Service>>.Success(result) : Result<List<Service>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}