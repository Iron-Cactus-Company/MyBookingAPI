using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.OpeningHoursActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public OpeningHours OpeningHours{ get; init; }
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
            var itemToUpdate = await _context.OpeningHours.FindAsync(request.OpeningHours.Id);
            if (itemToUpdate == null)
                return null;

            _mapper.Map(request.OpeningHours, itemToUpdate);
            
            var resp = ResponseDeterminer.DetermineUpdateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}