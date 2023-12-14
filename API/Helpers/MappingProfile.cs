using API.Contracts.BusinessProfile;
using API.Contracts.Client;
using API.Contracts.Company;
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
        
        
        CreateMap <CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
        CreateMap<Company, CompanyResponseObject>();
        
        
        CreateMap <CreateBusinessProfileDto, BusinessProfile>();
        CreateMap<UpdateBusinessProfileDto, BusinessProfile>();
        CreateMap<BusinessProfile, BusinessProfileResponseObject>();
        
        
     
    }
}

