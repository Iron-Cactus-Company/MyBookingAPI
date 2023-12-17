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
        CreateMap<UpdateCompanyDto, Company>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
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
        
        //Application layer mappings
        CreateMap<BusinessProfile, BusinessProfile>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
        CreateMap<Company, Company>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));
        CreateMap<OpeningHours, OpeningHours>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
        CreateMap<ExceptionHours, ExceptionHours>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
        CreateMap<Domain.Service, Domain.Service>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
        CreateMap<Booking, Booking>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
        CreateMap<Client, Client>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !(srcMember is Guid guid && guid == Guid.Empty)
            ));;
    }
}

