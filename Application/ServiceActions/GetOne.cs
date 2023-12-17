using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.ServiceActions;

public class GetOne
{
    public class Query : IRequest<Result<Service>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<Service>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Service>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.Service.FindAsync(request.Id);
           return result != null ? Result<Service>.Success(result) : Result<Service>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
        }
    }
}