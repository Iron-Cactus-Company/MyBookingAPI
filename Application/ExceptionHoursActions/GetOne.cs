using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.ExceptionHoursActions;

public class GetOne
{
    public class Query : IRequest<Result<ExceptionHours>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<ExceptionHours>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<ExceptionHours>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.ExceptionHours.FindAsync(request.Id);
           
           return Result<ExceptionHours>.Success(result);
        }
    }
}