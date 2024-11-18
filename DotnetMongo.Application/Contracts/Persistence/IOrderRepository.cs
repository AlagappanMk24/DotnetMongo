using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Persistence
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(string id);
        Task<Order> AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(string id);
        Task RemoveOrderItemReference(string orderId, string orderItemId);

    }
}
