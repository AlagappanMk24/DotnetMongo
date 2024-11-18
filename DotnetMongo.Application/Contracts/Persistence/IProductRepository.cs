using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Persistence
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(string id);
        Task<Product> AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string id);
    }
}
