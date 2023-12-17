using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.BusinessProfileActions;

public class GetOne
{
    public class Query : IRequest<Result<BusinessProfile>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<BusinessProfile>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<BusinessProfile>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.BusinessProfile.FindAsync(request.Id);
           return result != null ? Result<BusinessProfile>.Success(result) : Result<BusinessProfile>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
        }
    }
}