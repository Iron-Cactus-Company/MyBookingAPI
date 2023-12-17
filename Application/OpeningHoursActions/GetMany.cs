using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.OpeningHoursActions;

public class GetMany
{
    public class Query : IRequest<Result<List<OpeningHours>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<OpeningHours>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<OpeningHours>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.OpeningHours.ToListAsync();
            return result.Count() != 0 ? Result<List<OpeningHours>>.Success(result) : Result<List<OpeningHours>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}