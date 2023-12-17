using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyActions;

public class GetMany
{
    public class Query : IRequest<Result<List<Company>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<Company>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<Company>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Company.ToListAsync();
            return result.Count() != 0 ? Result<List<Company>>.Success(result) : Result<List<Company>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}