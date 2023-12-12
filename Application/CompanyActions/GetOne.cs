using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyActions;

public class GetOne
{
    public class Query : IRequest<Result<Company>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<Company>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Company>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.Company.FindAsync(request.Id);
           
           return Result<Company>.Success(result);
        }
    }
}