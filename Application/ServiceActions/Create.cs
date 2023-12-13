using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.ServiceActions;

public class Create
{ 
    public class Command : IRequest<Result<Unit>>
    {
        public Service Service{ get; init; }
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
            _context.Service.Add(request.Service);
            var result = await _context.SaveChangesAsync() > 0;
            return !result ? Result<Unit>.Failure("Could not create") : Result<Unit>.Success(Unit.Value);
        }
    }
}