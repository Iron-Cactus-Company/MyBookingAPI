using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.OpeningHoursActions;

public class FindOneByCompanyId
{
    public class Query : IRequest<Result<OpeningHours>>
    {
        public Guid CompanyId { get; init; }
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
            var result = await _context.OpeningHours.FirstOrDefaultAsync( item => item.CompanyId == request.CompanyId );
            
            return result is not null ? 
                Result<OpeningHours>.Success(result) : 
                Result<OpeningHours>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}