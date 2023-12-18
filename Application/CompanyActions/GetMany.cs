using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Company>>>
    {
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Company>>>
    {
        private readonly DataContext _context;
        private readonly OffsetPaginator<Company> _offsetPaginator = new ();

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Company>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(_context.Company, page, pageSize).ToListAsync();
            
            return result.Count() != 0 ? Result<List<Company>>.Success(result) : 
                Result<List<Company>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}