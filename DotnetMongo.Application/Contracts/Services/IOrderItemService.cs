using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Services
{
    public interface IOrderItemService
    {
        Task<Order> GetOrderItemsByOrderId(string orderId);
        Task<OrderItem> AddOrderItem(OrderItemDTO orderItemDto);
        Task<OrderItem> UpdateOrderItem(string id, OrderItemDTO orderDto);
        Task DeleteOrderItemAndUpdateOrder(string orderItemId);
    }
}
