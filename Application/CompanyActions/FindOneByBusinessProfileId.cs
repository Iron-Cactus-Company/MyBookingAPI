using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyActions;

public class FindOneByBusinessProfileId
{
    public class Query : IRequest<Result<Company>>
    {
        public Guid BusinessProfileId { get; init; }
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

            var result = await _context.Company.FirstOrDefaultAsync( item => item.BusinessProfileId == request.BusinessProfileId );
            
            return result is not null ? Result<Company>.Success(result) : 
                Result<Company>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}