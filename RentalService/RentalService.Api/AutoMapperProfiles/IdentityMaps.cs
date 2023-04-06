using AutoMapper;
using RentalService.Api.Contracts.IdentityContracts;
using RentalService.Application.Identity.Commands;

namespace RentalService.Api.AutoMapperProfiles;

public class IdentityMaps : Profile
{
    public IdentityMaps()
    {
        CreateMap<UserRegistration, RegisterIdentity>();
        CreateMap<UserLogin, LoginCommand>();
    }
}