using AutoMapper;
using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ForMember is used to handle custom property mapping.
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }
}