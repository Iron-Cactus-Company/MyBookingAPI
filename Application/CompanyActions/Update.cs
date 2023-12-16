using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyActions;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public Company Company{ get; init; }
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
            if (!GuidHandler.IsGuidNull(request.Company.BusinessProfileId))
            {
                var isBusinessProfileExists = await GuidHandler.IsEntityExists<BusinessProfile>(request.Company.BusinessProfileId, _context);

                if(!isBusinessProfileExists)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "BusinessProfile", Type = ErrorType.NotFound});
            }
            
            if (!string.IsNullOrEmpty(request.Company.Name))
            {
                var isNotUnique = await _context.Company.FirstOrDefaultAsync(item => item.Name == request.Company.Name) != null;
                if(isNotUnique)
                    return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Name", Type = ErrorType.NotUnique });
            }
            
            var itemToUpdate = await _context.Company.FindAsync(request.Company.Id);
            if (itemToUpdate == null)
                return Result<Unit>.Failure(new ApplicationRequestError{ Field = "Id", Type = ErrorType.NotFound});

            _mapper.Map(request.Company, itemToUpdate);

Console.WriteLine("-------------------------");
            Console.WriteLine(itemToUpdate.Id);
            Console.WriteLine(itemToUpdate.Email);
            Console.WriteLine("-------------------------");

        //TODO: Fix automapper bugs. It adds all properties to itemToUpdate, even if they are null
        //Probably there are 2 Automapper registrations 
            int respDB = 0;
            try
            {
                itemToUpdate.BusinessProfileId = new Guid("975de39d-c5ab-4902-96d2-0ba730f65f96");
                respDB = await _context.SaveChangesAsync();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            var resp = ResponseDeterminer.DetermineUpdateResponse(respDB);
            if (!resp.isValid)
                return Result<Unit>.Failure(resp.error);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}