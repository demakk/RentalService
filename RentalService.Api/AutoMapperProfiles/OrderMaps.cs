using AutoMapper;
using RentalService.Api.Contracts.OrderContracts.Responses;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Api.AutoMapperProfiles;

public class OrderMaps : Profile
{
    public OrderMaps()
    {
        CreateMap<Order, OrderResponse>();


        CreateMap<OrderItemLink, OrderItemLinkResponse>();
    }   
}