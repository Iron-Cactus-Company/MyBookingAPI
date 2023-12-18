using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class GetMany
{
    public class Query : IRequest<Result<List<BusinessProfile>>>
    {
        public ReadOptions Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<BusinessProfile>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<BusinessProfile> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<BusinessProfile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.BusinessProfile, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<BusinessProfile>>.Success(result) : Result<List<BusinessProfile>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}