using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ServiceActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Service>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<Service>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Service>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Service.ToListAsync();
            return Result<List<Service>>.Success(result);
        }
    }
}