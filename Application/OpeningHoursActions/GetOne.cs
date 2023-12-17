using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
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
           
           return result != null ? Result<OpeningHours>.Success(result) : Result<OpeningHours>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
        }
    }
}