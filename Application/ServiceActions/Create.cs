using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
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
            var isOpeningHoursExists = await GuidHandler.IsEntityExists<Company>(request.Service.CompanyId, _context);
            if(!isOpeningHoursExists)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "CompanyId", Type = ErrorType.NotFound});
            
            _context.Service.Add(request.Service);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}