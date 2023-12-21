using Application.AvailableHours;
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

public class FindByInterval
{
    public class Query : IRequest<Result<List<BookingDto>>>
    {
        public Guid CompanyId { get; init; }
        public Interval Interval { get; init; }
        public ReadOptions? Options { get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<BookingDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly OffsetPaginator<Booking> _offsetPaginator = new ();

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Result<List<BookingDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = _offsetPaginator.DeterminePageNumberAndSize(request.Options);

            var result = await _offsetPaginator.Paginate(
                _context.Booking
                    .Where(item => 
                        item.Service.CompanyId == request.CompanyId &&
                        item.Start >= request.Interval.Start &&
                        item.End <= request.Interval.End
                    ), 
                page, 
                pageSize
            ).ToListAsync();

            var bookingDtos = new List<BookingDto>();
            bookingDtos = _mapper.Map(result, bookingDtos);
            
            return result.Count() != 0 ? 
                Result<List<BookingDto>>.Success(bookingDtos) : 
                Result<List<BookingDto>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });
        }
    }
}