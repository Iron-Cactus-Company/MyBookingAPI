using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.OpeningHoursActions;

public class GetOne
{
    public class Query : IRequest<Result<OpeningHours>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<OpeningHours>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<OpeningHours>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.OpeningHours.FindAsync(request.Id);
           
           return Result<OpeningHours>.Success(result);
        }
    }
}