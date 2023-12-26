using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ServiceActions;

public class FindByCompanyId
{
    public class Query : IRequest<Result<List<Service>>>
    {
        public Guid CompanyId { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Service>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Service>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Service.Where( item => item.CompanyId == request.CompanyId ).ToListAsync();
            
            return result is not null ? 
                Result<List<Service>>.Success(result) : 
                Result<List<Service>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}