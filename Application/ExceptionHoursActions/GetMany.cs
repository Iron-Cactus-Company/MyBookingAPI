using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ExceptionHoursActions;

public class GetMany
{
    public class Query : IRequest<Result<List<ExceptionHours>>>{}
    
    public class Handler : IRequestHandler<Query, Result<List<ExceptionHours>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<ExceptionHours>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.ExceptionHours.ToListAsync();
            return result.Count() != 0 ? Result<List<ExceptionHours>>.Success(result) : Result<List<ExceptionHours>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}