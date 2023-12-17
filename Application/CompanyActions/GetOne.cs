using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
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
           
           return result != null ? Result<Company>.Success(result) : Result<Company>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
        }
    }
}