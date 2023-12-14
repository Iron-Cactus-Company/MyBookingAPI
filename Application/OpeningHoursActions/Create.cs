using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.OpeningHoursActions;

public class Create
{ 
    public class Command : IRequest<Result<Unit>>
    {
        public OpeningHours OpeningHours{ get; init; }
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
            _context.OpeningHours.Add(request.OpeningHours);
            var result = await _context.SaveChangesAsync() > 0;
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}