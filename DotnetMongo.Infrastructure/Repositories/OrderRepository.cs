using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Domain.Models.Entities;
using DotnetMongo.Infrastructure.Data.Context;
using DotnetMongo.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetMongo.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // Way 1 : Using Db Context and mongodb configuration settings
        private readonly ApplicationDbContext _context;
        public OrderRepository(IOptions<MongoDbConfiguration> settings)
        {
            _context = new ApplicationDbContext(settings);
        }
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.OrderCollection.Find(_ => true).ToListAsync();
        }

        // Get order by id
        public async Task<Order> GetOrderById(string id)
        {
            return await _context.OrderCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        // Add a new order
        public async Task<Order> AddOrder(Order order)
        {
            await _context.OrderCollection.InsertOneAsync(order); // InsertOneAsync for MongoDB
            return order;
        }
        // Update an order
        public async Task UpdateOrder(Order order)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
            var update = Builders<Order>.Update
                .Set(o => o.OrderDate, order.OrderDate)
                .Set(o => o.CustomerName, order.CustomerName)
                 .Set(o => o.OrderItemIds, order.OrderItemIds); // Update the OrderItemIds list

            await _context.OrderCollection.UpdateOneAsync(filter, update); // UpdateOneAsync for MongoDB
        }

        // Delete an order by id
        public async Task DeleteOrder(string id)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            await _context.OrderCollection.DeleteOneAsync(filter); // DeleteOneAsync for MongoDB
        }

        public async Task RemoveOrderItemReference(string orderId, string orderItemId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
            var update = Builders<Order>.Update.Pull(o => o.OrderItemIds, orderItemId); // Remove the specific order item ID
            await _context.OrderCollection.UpdateOneAsync(filter, update);
        }

    }
}

#region Way 2 : Directly injected without dbcontext and mongodb configuration settings
//private readonly IMongoCollection<Order> _orderCollection;
//private readonly MongoDbConfiguration _dbConfigurationSettings;
//public OrderRepository(IMongoClient client)
//{
//    var database = client.GetDatabase("ProductOrder");
//    _orderCollection = database.GetCollection<Order>(nameof(Order));
//}
//// Get all orders
//public async Task<IEnumerable<Order>> GetAllOrders()
//{
//    return await _orderCollection.Find(_ => true).ToListAsync();
//}

//// Get order by id
//public async Task<Order> GetOrderById(string id)
//{
//    return await _orderCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
//}
//// Add a new order
//public async Task<Order> AddOrder(Order order)
//{
//    await _orderCollection.InsertOneAsync(order); // InsertOneAsync for MongoDB
//    return order;
//}
//// Update an order
//public async Task UpdateOrder(Order order)
//{
//    var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
//    var update = Builders<Order>.Update
//        .Set(o => o.OrderDate, order.OrderDate)
//        .Set(o => o.CustomerName, order.CustomerName)
//         .Set(o => o.OrderItemIds, order.OrderItemIds); // Update the OrderItemIds list

//    await _orderCollection.UpdateOneAsync(filter, update); // UpdateOneAsync for MongoDB
//}

//// Delete an order by id
//public async Task DeleteOrder(string id)
//{
//    var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
//    await _orderCollection.DeleteOneAsync(filter); // DeleteOneAsync for MongoDB
//}

//public async Task RemoveOrderItemReference(string orderId, string orderItemId)
//{
//    var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
//    var update = Builders<Order>.Update.Pull(o => o.OrderItemIds, orderItemId); // Remove the specific order item ID
//    await _orderCollection.UpdateOneAsync(filter, update);
//}
#endregion