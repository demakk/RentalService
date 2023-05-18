using AutoMapper;
using RentalService.Api.Contracts.UserProfileContracts.Requests;
using RentalService.Api.Contracts.UserProfileContracts.Responses;
using RentalService.Application.UserProfiles.CommandHandlers;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Domain.Aggregates.Common;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Api.AutoMapperProfiles;

public class UserProfileMaps : Profile
{
    public UserProfileMaps()
    {
        CreateMap<UserProfileCreate, UpdateUserProfileCommand>();
        CreateMap<UserBasicInfo, BasicInfoResponse>();
        CreateMap<UserProfile, UserProfileResponse>()
            .ForMember(
                dest => dest.BasicInfoResponse,
                opt => 
                opt.MapFrom(src => src.UserBasicInfo));

    }
}