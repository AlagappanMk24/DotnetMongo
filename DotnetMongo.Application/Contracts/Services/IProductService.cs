using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Contracts.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(string id);
        Task<Product> AddProduct(ProductDto product);
        Task<Product> UpdateProduct(string id, ProductDto productDto);
        Task DeleteProduct(string id);
    }
}
