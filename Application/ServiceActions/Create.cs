using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.ServiceActions;

public class Create
{ 
    public class Command : IRequest<Result<Service>>
    {
        public Service Service{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<Service>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Service>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isOpeningHoursExists = await GuidHandler.IsEntityExists<Company>(request.Service.CompanyId, _context);
            if(!isOpeningHoursExists)
                return Result<Service>.Failure(new ApplicationRequestError{ Field = "CompanyId", Type = ErrorType.NotFound});
            
            _context.Service.Add(request.Service);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<Service>.Failure(resp.error);
            
            return Result<Service>.Success(request.Service);
        }
    }
}