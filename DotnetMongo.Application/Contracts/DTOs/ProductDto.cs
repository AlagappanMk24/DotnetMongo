using System.ComponentModel.DataAnnotations;

namespace DotnetMongo.Application.Contracts.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        // Description is optional but should not exceed 500 characters
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        // Price is required and must be a positive value
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}