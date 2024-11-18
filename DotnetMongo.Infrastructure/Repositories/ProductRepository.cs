using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Domain.Models.Entities;
using DotnetMongo.Infrastructure.Data.Context;
using DotnetMongo.Infrastructure.Repositories;
using DotnetMongo.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetMongo.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Way 1 : Using Db Context and mongodb configuration settings
        private readonly ApplicationDbContext _context;
        public ProductRepository(IOptions<MongoDbConfiguration> settings)
        {
            _context = new ApplicationDbContext(settings);
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.ProductCollection.Find(_ => true).ToListAsync();
        }
        public async Task<Product> GetProductById(string id)
        {
            return await _context.ProductCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Product> AddProduct(Product product)
        {
            await _context.ProductCollection.InsertOneAsync(product); // InsertOneAsync for MongoDB
            return product;
        }
        public async Task UpdateProduct(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            var update = Builders<Product>.Update
                .Set(p => p.Name, product.Name)
                .Set(p => p.Description, product.Description)
                .Set(p => p.Price, product.Price); // Update the OrderItemIds list

            await _context.ProductCollection.UpdateOneAsync(filter, update); // UpdateOneAsync for MongoDB

        }
        public async Task DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(o => o.Id, id);
            await _context.ProductCollection.DeleteOneAsync(filter); // DeleteOneAsync for MongoDB
        }
    }
}

#region Way 2 : Directly injected without dbcontext and mongodb configuration settings
//private readonly IMongoCollection<Product> _productCollection;
//private readonly MongoDbConfiguration _dbConfigurationSettings;
//public ProductRepository(IMongoClient client)
//{
//    var database = client.GetDatabase("ProductOrder");
//    _productCollection = database.GetCollection<Product>(nameof(Product));
//}
//public async Task<IEnumerable<Product>> GetAllProducts()
//{
//    return await _productCollection.Find(_ => true).ToListAsync();
//}
//public async Task<Product> GetProductById(string id)
//{
//    return await _productCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
//}
//public async Task<Product> AddProduct(Product product)
//{
//    await _productCollection.InsertOneAsync(product); // InsertOneAsync for MongoDB
//    return product;
//}
//public async Task UpdateProduct(Product product)
//{
//    var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
//    var update = Builders<Product>.Update
//        .Set(p => p.Name, product.Name)
//        .Set(p => p.Description, product.Description)
//        .Set(p => p.Price, product.Price); // Update the OrderItemIds list

//    await _productCollection.UpdateOneAsync(filter, update); // UpdateOneAsync for MongoDB

//}
//public async Task DeleteProduct(string id)
//{
//    var filter = Builders<Product>.Filter.Eq(o => o.Id, id);
//    await _productCollection.DeleteOneAsync(filter); // DeleteOneAsync for MongoDB
//}
#endregion