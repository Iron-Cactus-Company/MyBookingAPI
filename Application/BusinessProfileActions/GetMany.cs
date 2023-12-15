using Application.Core;
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
            if(request.Options.PageNumber == null && request.Options.Limit == null){
                var noPagination = await _context.BusinessProfile.ToListAsync();
                return Result<List<BusinessProfile>>.Success(noPagination);
            }
            
            var result = await _offsetPaginator
            .paginate(_context.BusinessProfile, (int)request.Options.PageNumber, (int)request.Options.Limit);
            return Result<List<BusinessProfile>>.Success(result);
            
            //var currentPosition = request.Options.PageNumber*request.Options.Limit;
            /*var result = await _context.BusinessProfile
                .OrderBy(item => item.Id)
               .Skip(currentPosition)
                //.Take(request.Options.Limit)
                .ToListAsync(); */

            //return Result<List<BusinessProfile>>.Success(null);*/
        }
    }
}