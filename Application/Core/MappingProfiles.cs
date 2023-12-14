using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<BusinessProfile, BusinessProfile>();
        CreateMap<Company, Company>();
        CreateMap<OpeningHours, OpeningHours>();
        CreateMap<ExceptionHours, ExceptionHours>();
        CreateMap<Service, Service>();
        CreateMap<Booking, Booking>();
        CreateMap<Client, Client>();
    }
}