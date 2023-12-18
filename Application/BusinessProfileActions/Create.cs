using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Application.Core.Security;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BusinessProfileActions;

public class Create
{ 
    public class Command : IRequest<Result<BusinessProfile>>
    {
        public BusinessProfile BusinessProfile{ get; init; }
    } 
    
    public class Handler : IRequestHandler<Command, Result<BusinessProfile>>
    {
        private readonly DataContext _context;
        private readonly PasswordManager _passwordManager;

        public Handler(DataContext context, PasswordManager passwordManager)
        {
            _context = context;
            _passwordManager = passwordManager;
        }

        public async Task<Result<BusinessProfile>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isNotUnique = await _context.BusinessProfile.FirstOrDefaultAsync(item => item.Email == request.BusinessProfile.Email) != null;
            if(isNotUnique)
                return Result<BusinessProfile>.Failure(new ApplicationRequestError{ Field = "Email", Type = ErrorType.NotUnique });

            var hashedPassword = _passwordManager.HashPassword(request.BusinessProfile.Password);
            request.BusinessProfile.Password = hashedPassword;
            
            _context.BusinessProfile.Add(request.BusinessProfile);
            var resp = ResponseDeterminer.DetermineCreateResponse(await _context.SaveChangesAsync());
            if (!resp.isValid)
                return Result<BusinessProfile>.Failure(resp.error);
            
            return Result<BusinessProfile>.Success(request.BusinessProfile);
        }
    }
}