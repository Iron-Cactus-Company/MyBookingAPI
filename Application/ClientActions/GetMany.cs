using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ClientActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Client>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Client>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<Client> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Client>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.Client, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<Client>>.Success(result) : Result<List<Client>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}