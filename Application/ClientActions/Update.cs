using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.ClientActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public Booking Client { get; init; }
    }
    
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _context.Client.FindAsync(request.Client.Id);
            if (itemToUpdate == null)
                return null;

            _mapper.Map(request.Client, itemToUpdate);
            
            var resp = ResponseDeterminer.DetermineUpdateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}