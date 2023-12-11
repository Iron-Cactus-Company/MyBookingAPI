using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class GetMany
{
    public class Query : IRequest<Result<List<BusinessProfile>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<BusinessProfile>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        //cancellation token can be used for cancelling the request.
        //Cancelling may happen if the user close the browser,
        //for example if something took too long to be processed
        //U may get that parameter in the controller and get it here
        public async Task<Result<List<BusinessProfile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.BusinessProfile.ToListAsync();
            return Result<List<BusinessProfile>>.Success(result);
        }
    }
}