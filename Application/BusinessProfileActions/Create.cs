using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class Create
{ 
    public class Command : IRequest<Result<Unit>>
    {
        public BusinessProfile BusinessProfile{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isNotUnique = await _context.BusinessProfile.FirstOrDefaultAsync(item => item.Email == request.BusinessProfile.Email) != null;
            if(isNotUnique)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Email", Type = ErrorType.NotUnique}); 
            
            _context.BusinessProfile.Add(request.BusinessProfile);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}