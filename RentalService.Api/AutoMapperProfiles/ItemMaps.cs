using AutoMapper;
using RentalService.Api.Contracts.ItemContracts.Requests;
using RentalService.Api.Contracts.ItemContracts.Responses;
using RentalService.Application.Items.Commands;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Api.AutoMapperProfiles;

public class ItemMaps : Profile
{
    public ItemMaps()
    {
        CreateMap<Item, ItemResponse>();
        CreateMap<ItemCreate, CreateItemCommand>();
        CreateMap<ItemUpdate, UpdateItemCommand>();
    }
}