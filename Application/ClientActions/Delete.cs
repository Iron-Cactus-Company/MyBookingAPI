using Application.Core;
using MediatR;
using Persistence;

namespace Application.ClientActions;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; init; }
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
            var itemToDelete = await _context.Client.FindAsync(request.Id);
            if(itemToDelete == null)
                return null;
            
            _context.Remove(itemToDelete);
            
            var resp = ResponseDeterminer.DetermineDeleteResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}