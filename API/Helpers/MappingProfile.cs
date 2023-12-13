using API.Contracts.Client;
using API.Contracts.Company;
using AutoMapper;

namespace API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCompanyDto, CompanyResponseObject>();
        CreateMap<UpdateCompanyDto, CompanyResponseObject>();
        CreateMap<CreateClientDto, ClientResponseObject>();
        CreateMap<UpdateClientDto, ClientResponseObject>();
    }
}