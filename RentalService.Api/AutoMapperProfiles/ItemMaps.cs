using AutoMapper;
using RentalService.Api.Contracts.ItemContracts.Responses;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Api.AutoMapperProfiles;

public class ItemMaps : Profile
{
    public ItemMaps()
    {
        CreateMap<Item, ItemResponse>();
    }
}