using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public BusinessProfile BusinessProfile{ get; init; }
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
            if (!string.IsNullOrEmpty(request.BusinessProfile.Email))
            {
                var isNotUnique = await _context.BusinessProfile
                    .FirstOrDefaultAsync(item => item.Email == request.BusinessProfile.Email && item.Id != request.BusinessProfile.Id) != null;
                if(isNotUnique)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Email", Type = ErrorType.NotUnique });
            }
            
            var itemToUpdate = await _context.BusinessProfile.FindAsync(request.BusinessProfile.Id);
            if (itemToUpdate == null)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Id", Type = ErrorType.NotFound});

            _mapper.Map(request.BusinessProfile, itemToUpdate);
            
            var resp = ResponseDeterminer.DetermineUpdateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}