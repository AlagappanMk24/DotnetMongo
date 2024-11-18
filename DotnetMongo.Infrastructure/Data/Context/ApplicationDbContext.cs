using DotnetMongo.Domain.Models.Entities;
using DotnetMongo.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetMongo.Infrastructure.Data.Context
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IOptions<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Product> ProductCollection =>
            _database.GetCollection<Product>(nameof(Product));
        public IMongoCollection<Order> OrderCollection =>
         _database.GetCollection<Order>(nameof(Order));
        public IMongoCollection<OrderItem> OrderItemCollection =>
         _database.GetCollection<OrderItem>(nameof(OrderItem));
    }
}
