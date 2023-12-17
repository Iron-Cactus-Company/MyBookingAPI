using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class FindOneByEmail
{
    public class Query : IRequest<Result<BusinessProfile>>
    {
        public string Email{ get; init; }
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
            var result = await _context.BusinessProfile.FirstOrDefaultAsync(item => item.Email == request.Email);
           
            return result != null ? Result<BusinessProfile>.Success(result) : Result<BusinessProfile>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Email" });
        }
    }
}