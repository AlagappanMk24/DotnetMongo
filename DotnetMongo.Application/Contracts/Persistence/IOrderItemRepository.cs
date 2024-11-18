using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Persistence
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetOrderItemsByIds(List<string> orderItemIds);
        Task<OrderItem> GetOrderItemById(string id);
        Task AddOrderItem(OrderItem orderItem);
        Task UpdateOrderItem(OrderItem orderItem);
        Task DeleteOrderItem(string id);
        Task DeleteOrderItemsByIds(IEnumerable<string> orderItemIds);
    }
}
