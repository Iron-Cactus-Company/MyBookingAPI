using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ClientActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public Client Client { get; init; }
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
            if (!string.IsNullOrEmpty(request.Client.Email))
            {
                var isNotUnique = await _context.Client
                    .FirstOrDefaultAsync(item => item.Email == request.Client.Email && item.Id == request.Client.Id ) != null;
                
                if(isNotUnique)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Email", Type = ErrorType.NotUnique });
            }
            
            var itemToUpdate = await _context.Client.FindAsync(request.Client.Id);
            if (itemToUpdate == null)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Id", Type = ErrorType.NotFound });

            _mapper.Map(request.Client, itemToUpdate);
            
            var resp = ResponseDeterminer.DetermineUpdateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}