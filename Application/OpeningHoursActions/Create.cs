using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.OpeningHoursActions;

public class Create
{ 
    public class Command : IRequest<Result<OpeningHours>>
    {
        public OpeningHours OpeningHours{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<OpeningHours>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<OpeningHours>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isOpeningHoursExists = await GuidHandler.IsEntityExists<Company>(request.OpeningHours.CompanyId, _context);
            if(!isOpeningHoursExists)
                return Result<OpeningHours>.Failure(new ApplicationRequestError{ Field = "CompanyId", Type = ErrorType.NotFound});
            
            _context.OpeningHours.Add(request.OpeningHours);
            var result = await _context.SaveChangesAsync() > 0;
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<OpeningHours>.Failure(resp.error);
            
            return Result<OpeningHours>.Success(request.OpeningHours);
        }
    }
}