using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.ClientActions;

public class GetOne
{
    public class Query : IRequest<Result<Client>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<Client>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Client>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.Client.FindAsync(request.Id);
           
           return result != null ? Result<Client>.Success(result) : Result<Client>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
        }
    }
}