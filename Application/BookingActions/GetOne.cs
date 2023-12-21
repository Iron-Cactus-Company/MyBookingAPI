using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using Application.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.BookingActions;

public class GetOne
{
    public class Query : IRequest<Result<BookingDto>>
    {
        public Guid Id{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<BookingDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<BookingDto>> Handle(Query request, CancellationToken cancellationToken)
        {
           var result = await _context.Booking
               .Include(item => item.Client)
               .FirstOrDefaultAsync(item => item.Id == request.Id);

            if(result == null)
                return Result<BookingDto>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "Id" });
            
           return Result<BookingDto>.Success(_mapper.Map<BookingDto>(result));
        }
    }
}