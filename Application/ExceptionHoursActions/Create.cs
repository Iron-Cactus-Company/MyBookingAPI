using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Domain;
using MediatR;
using Persistence;

namespace Application.ExceptionHoursActions;

public class Create
{ 
    public class Command : IRequest<Result<ExceptionHours>>
    {
        public ExceptionHours ExceptionHours{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<ExceptionHours>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<ExceptionHours>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isOpeningHoursExists = await GuidHandler.IsEntityExists<OpeningHours>(request.ExceptionHours.OpeningHoursId, _context);
            if(!isOpeningHoursExists)
                return Result<ExceptionHours>.Failure(new ApplicationRequestError{ Field = "OpeningHoursId", Type = ErrorType.NotFound});
            
            _context.ExceptionHours.Add(request.ExceptionHours);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<ExceptionHours>.Failure(resp.error);
            
            return Result<ExceptionHours>.Success(request.ExceptionHours);
        }
    }
}