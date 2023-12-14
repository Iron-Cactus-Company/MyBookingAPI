using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyActions;

public class Create
{ 
    public class Command : IRequest<Result<Unit>>
    {
        public Company Company{ get; init; }
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
            if (!GuidHandler.IsGuidNull(request.Company.BusinessProfileId))
            {
                var isBusinessProfileExists = await GuidHandler.IsEntityExists<BusinessProfile>(request.Company.BusinessProfileId, _context);
                if(!isBusinessProfileExists)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "BusinessProfile", Type = ErrorType.NotFound});
            }
            
            var isNotUnique = await _context.Company.FirstOrDefaultAsync(item => item.Name == request.Company.Name) != null;
            if(isNotUnique)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Name", Type = ErrorType.NotUnique });
            
            _context.Company.Add(request.Company);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}