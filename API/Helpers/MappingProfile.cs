using API.Contracts.Auth;
using API.Contracts.Booking;
using API.Contracts.BusinessProfile;
using API.Contracts.Client;
using API.Contracts.Company;
using API.Contracts.ExceptionHours;
using API.Contracts.OpeningHours;
using API.Contracts.Service;
using AutoMapper;
using Domain;
namespace API.Helpers;

public class MappingProfile : Profile
{
    
    public MappingProfile()
    {
        CreateMap <BusinessProfile, LoginUserResponseObject>();
        
        CreateMap <CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();
        CreateMap<Client, ClientResponseObject>();
       
        
        CreateMap <CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
        CreateMap<Company, CompanyResponseObject>();
        
        
        CreateMap <CreateBusinessProfileDto, BusinessProfile>();
        CreateMap<UpdateBusinessProfileDto, BusinessProfile>();
        CreateMap<BusinessProfile, BusinessProfileResponseObject>();
        
        
        CreateMap <CreateBookingDto, Booking>();
        CreateMap<UpdateBookingDto, Booking>();
        CreateMap<Booking, BookingResponseObject>();
        
        CreateMap <CreateServiceDto, Domain.Service>();
        CreateMap<UpdateServiceDto, Domain.Service>();
        CreateMap<Domain.Service, ServiceResponseObject>();
        // .ForMember(dest => dest.Company,
        //     opt
        //         => opt.MapFrom(src
        //             => src.Company));

        
        CreateMap <CreateOpeningHoursDto, OpeningHours>();
        CreateMap<UpdateOpeningHoursDto, OpeningHours>();
        CreateMap<OpeningHours, OpeningHoursResponseObject>();
        
        
        CreateMap <CreateExceptionHoursDto, ExceptionHours>();
        CreateMap<UpdateExceptionHoursDto, ExceptionHours>();
        CreateMap<ExceptionHours, ExceptionHoursResponseObject>();
        
        
    }
}

