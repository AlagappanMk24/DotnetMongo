using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Domain.Models.Entities;
using DotnetMongo.Infrastructure.Data.Context;
using DotnetMongo.Infrastructure.Repositories;
using DotnetMongo.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetMongo.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        // Way 1 : Using Db Context and mongodb configuration settings
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(IOptions<MongoDbConfiguration> settings)
        {
            _context = new ApplicationDbContext(settings);
        }
        public async Task<List<OrderItem>> GetOrderItemsByIds(List<string> orderItemIds)
        {
            // Find order items by a list of order item Ids
            var filter = Builders<OrderItem>.Filter.In(oi => oi.Id, orderItemIds);
            return await _context.OrderItemCollection.Find(filter).ToListAsync();
        }
        public async Task<OrderItem> GetOrderItemById(string id)
        {
            return await _context.OrderItemCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddOrderItem(OrderItem orderItem)
        {
            await _context.OrderItemCollection.InsertOneAsync(orderItem); // Insert the OrderItem in the database
        }

        // Update order items
        public async Task UpdateOrderItem(OrderItem orderItem)
        {
            var filter = Builders<OrderItem>.Filter.Eq(oi => oi.Id, orderItem.Id);
            var update = Builders<OrderItem>.Update
                .Set(oi => oi.Quantity, orderItem.Quantity)
                .Set(oi => oi.ProductId, orderItem.ProductId);

            await _context.OrderItemCollection.UpdateOneAsync(filter, update);
        }
        public async Task DeleteOrderItem(string id)
        {
            // Delete an order item by its Id
            var filter = Builders<OrderItem>.Filter.Eq(oi => oi.Id, id);
            await _context.OrderItemCollection.DeleteOneAsync(filter);
        }

        // Delete all order items by order id
        public async Task DeleteOrderItemsByIds(IEnumerable<string> orderItemIds)
        {
            var filter = Builders<OrderItem>.Filter.In(o => o.Id, orderItemIds);
            await _context.OrderItemCollection.DeleteManyAsync(filter); // Delete all matching OrderItems
        }
    }
}

#region Way 2 : Directly injected without dbcontext and mongodb configuration settings
//private readonly IMongoCollection<OrderItem> _orderItemCollection;
//private readonly MongoDbConfiguration _dbConfigurationSettings;
//public OrderItemRepository(IMongoClient client)
//{
//    var database = client.GetDatabase("ProductOrder");
//    _orderItemCollection = database.GetCollection<OrderItem>(nameof(OrderItem));
//}
//public async Task<List<OrderItem>> GetOrderItemsByIds(List<string> orderItemIds)
//{
//    // Find order items by a list of order item Ids
//    var filter = Builders<OrderItem>.Filter.In(oi => oi.Id, orderItemIds);
//    return await _orderItemCollection.Find(filter).ToListAsync();
//}
//public async Task<OrderItem> GetOrderItemById(string id)
//{
//    return await _orderItemCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
//}
//public async Task AddOrderItem(OrderItem orderItem)
//{
//    await _orderItemCollection.InsertOneAsync(orderItem); // Insert the OrderItem in the database
//}

//// Update order items
//public async Task UpdateOrderItem(OrderItem orderItem)
//{
//    var filter = Builders<OrderItem>.Filter.Eq(oi => oi.Id, orderItem.Id);
//    var update = Builders<OrderItem>.Update
//        .Set(oi => oi.Quantity, orderItem.Quantity)
//        .Set(oi => oi.ProductId, orderItem.ProductId);

//    await _orderItemCollection.UpdateOneAsync(filter, update);
//}
//public async Task DeleteOrderItem(string id)
//{
//    // Delete an order item by its Id
//    var filter = Builders<OrderItem>.Filter.Eq(oi => oi.Id, id);
//    await _orderItemCollection.DeleteOneAsync(filter);
//}

//// Delete all order items by order id
//public async Task DeleteOrderItemsByIds(IEnumerable<string> orderItemIds)
//{
//    var filter = Builders<OrderItem>.Filter.In(o => o.Id, orderItemIds);
//    await _orderItemCollection.DeleteManyAsync(filter); // Delete all matching OrderItems
//}
#endregion