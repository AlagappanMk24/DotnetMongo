using AutoMapper;
using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Infrastructure.Services
{
    public class ProductService(IMapper mapper, IProductRepository productRepository) : IProductService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products;
        }
        public async Task<Product> GetProductById(string id)
        {
            var product = await _productRepository.GetProductById(id);
            return product;
        }

        public async Task<Product> AddProduct(ProductDto productDto)
        {
            ArgumentNullException.ThrowIfNull(productDto);

            // Map the DTO to the entity using AutoMapper
            var product = _mapper.Map<Product>(productDto);

            // Add the mapped order to the database via the Unit of Work pattern
            var createdProduct = await _productRepository.AddProduct(product);

            return createdProduct;
        }

        public async Task<Product> UpdateProduct(string id, ProductDto productDto)
        {
            // Fetch the existing product from the repository
            var existingProduct = await _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                return null;
            }

            // Map the ProductDto to the existing product
            _mapper.Map(productDto, existingProduct); // Only updates properties, does not overwrite object

            // Update the product in the repository
            await _productRepository.UpdateProduct(existingProduct);

            return existingProduct;
        }

        public async Task DeleteProduct(string id)
        {
            // Perform the delete operation
            await _productRepository.DeleteProduct(id);
        }
    }
}
