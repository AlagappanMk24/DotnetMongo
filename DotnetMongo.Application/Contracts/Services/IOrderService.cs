using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(string id);
        Task<Order> AddOrder(OrderDto orderDto);
        Task<Order> UpdateOrder(string id, OrderDto orderDto);
        Task DeleteOrder(string orderId);
    }
}
