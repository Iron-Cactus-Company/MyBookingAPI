using API.Contracts.Client;
using AutoMapper;
using Domain;
namespace API.Helpers;

public class MappingProfile : Profile
{
    
    public MappingProfile()
    {
        CreateMap <CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();
        CreateMap<Client, ClientResponseObject>();
        
        
        // CreateMap<List<Client>, List<ClientResponseObject>>();
        // CreateMap<MyStuffDTO, MyStuffViewModel>()
        //     .ForMember(dto => dto.MyDate, opt => opt.MapFrom(src => src.LastDate))
        //     .ForMember(dto => dto.MyTime, opt => opt.MapFrom(src => src.LastTime))
        //     .ForMember(dto => dto.Category, opt => opt.MapFrom(src => src.Category));
        
        
        // CreateMap<CreateCompanyDto, CompanyResponseObject>();
        // CreateMap<UpdateCompanyDto, CompanyResponseObject>();
        // CreateMap<CreateClientDto, ClientResponseObject>();
        // CreateMap<UpdateClientDto, ClientResponseObject>();
    }
}

