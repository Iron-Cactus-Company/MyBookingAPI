using Application.Core;
using Application.Core.Error;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.AvailableHours;

public class GetAll
{
    public class Query : IRequest<Result<List<Interval>>>
    {
        public Guid ServiceId{ get; init; }
        
        public long From{ get; init; }
        public long To{ get; init; }
    }
    
    public class Handler : IRequestHandler<Query, Result<List<Interval>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<Interval>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if(request.From >= request.To)
                return Result<List<Interval>>.Failure(new ApplicationRequestError{ Type = ErrorType.InvalidRequest, Field = "From" });
            
            var service = await _context.Service.FindAsync(request.ServiceId);
            if(service == null) 
                return Result<List<Interval>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "ServiceId" });
            
            var openingHours = await _context.OpeningHours.FirstOrDefaultAsync(item => item.CompanyId == service.CompanyId);
            if(openingHours == null) 
                return Result<List<Interval>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "OpeningHours.CompanyId" });
            
            var allCompanyServices = await _context.Service.Where(item => item.CompanyId == service.CompanyId).ToListAsync();

            var bookings = new List<Booking>();
            foreach (var s in allCompanyServices)
            {
                var bookingsOfS = await _context.Booking
                    .Where(item => 
                        item.ServiceId == s.Id && 
                        item.Start >= request.From && 
                        item.End <= request.To)
                    .ToListAsync();
                
                bookings.AddRange(bookingsOfS);
            }

            var requestDateTime = UnixTimeStampToDateTime(request.From);
            var openHoursInterval = GetOpenHoursInterval(openingHours, requestDateTime, new Interval(request.From, request.To));

            if (openHoursInterval is null)
                return Result<List<Interval>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound, Field = "From" });

            var bookingIntervals = ConvertBookingsToIntervals(bookings);

            var availableTimeIntervals = new List<Interval> { openHoursInterval };
            foreach (var booking in bookingIntervals)
            {
                var newIntervals = new List<Interval>();
                foreach (var availableTime in availableTimeIntervals)
                {
                    var (first, second) = availableTime.RemoveOverlapping(booking);
                    //If booking overlap the whole availableTime
                    if (first == null && second == null)
                    {
                        continue;
                    }
                    
                    //If booking inside availableTime
                    if (first != null && second != null)
                    {
                        newIntervals.Add(first);
                        newIntervals.Add(second);
                        break;
                    }

                    if (first != null)
                    {
                        newIntervals.Add(first);
                    }
                }
                
                availableTimeIntervals = newIntervals;
            }

            var dividedForServiceDurationIntervals = new List<Interval>();
            foreach (var interval in availableTimeIntervals)
            {
                var dividedIntervals = DivideInToParts(interval, service.Duration);
                dividedForServiceDurationIntervals.AddRange(dividedIntervals);
            }
            
            return dividedForServiceDurationIntervals.Count() != 0 ? 
                Result<List<Interval>>.Success(dividedForServiceDurationIntervals) : 
                Result<List<Interval>>.Failure(new ApplicationRequestError{ Type = ErrorType.NotFound });;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        private static long ConvertToTimestamp(DateTime value)
        {
            var elapsedTime = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) elapsedTime.TotalSeconds;
        }
        
        private Interval GetOpenHoursInterval(OpeningHours hours, DateTime requestDate, Interval requestedInterval)
        {
            var day = requestDate.DayOfWeek;
            var hmStringStart = "0:0";
            var hmStringEnd = "0:0";

            if (day == DayOfWeek.Monday)
            {
                hmStringStart = hours.MondayStart;
                hmStringEnd = hours.MondayEnd;
            } 
            else if (day == DayOfWeek.Tuesday)
            {
                hmStringStart = hours.TuesdayStart;
                hmStringEnd = hours.TuesdayEnd;
            }
            else if (day == DayOfWeek.Wednesday)
            {
                hmStringStart = hours.WednesdayStart;
                hmStringEnd = hours.WednesdayEnd;
            }
            else if (day == DayOfWeek.Thursday)
            {
                hmStringStart = hours.ThursdayStart;
                hmStringEnd = hours.ThursdayEnd;
            }
            else if (day == DayOfWeek.Friday)
            {
                hmStringStart = hours.FridayStart;
                hmStringEnd = hours.FridayEnd;
            }
            else if (day == DayOfWeek.Saturday)
            {
                hmStringStart = hours.SaturdayStart;
                hmStringEnd = hours.SaturdayEnd;
            }
            else if (day == DayOfWeek.Sunday)
            {
                hmStringStart = hours.SundayStart;
                hmStringEnd = hours.SundayEnd;
            }

            var startSplit = hmStringStart.Split(":");
            var endSplit = hmStringEnd.Split(":");

            int startHour = 0, endHour = 0, startMin = 0, endMin = 0;
            if (startSplit.Length == 2 && endSplit.Length == 2)
            {
                 int.TryParse(startSplit[0], out startHour);
                 int.TryParse(startSplit[1], out startMin);
                 int.TryParse(endSplit[0], out endHour);
                 int.TryParse(endSplit[1], out endMin);
            }
            
            var ohStartDate = new DateTime(requestDate.Year, requestDate.Month, requestDate.Day, startHour, startMin, 0, 0, DateTimeKind.Utc);
            var ohEndDate = new DateTime(requestDate.Year, requestDate.Month, requestDate.Day, endHour, endMin, 0, 0, DateTimeKind.Utc);
            var ohInterval = new Interval(ConvertToTimestamp(ohStartDate), ConvertToTimestamp(ohEndDate));

            return ohInterval.GetOverlapping(requestedInterval);
        }

        private List<Interval> ConvertBookingsToIntervals(List<Booking> bookings)
        {
            var bookingIntervals = new List<Interval>();
            foreach (var booking in bookings)
                bookingIntervals.Add(new Interval(booking.Start, booking.End));

            return bookingIntervals;
        }
        
        private List<Interval> DivideInToParts(Interval interval, long partDuration)
        {
            var parts = new List<Interval>();
            var nextStart = interval.Start;
            while (nextStart+partDuration <= interval.End)
            {
                var nextEnd = nextStart + partDuration;
                parts.Add(new Interval(nextStart, nextEnd));
                nextStart = nextEnd;
            }

            return parts;
        }
    }
}