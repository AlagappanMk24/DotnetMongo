using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetMongo.Application.Contracts.DTOs
{
    public class OrderDto
    {
        [Required(ErrorMessage = "OrderDate is required.")]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "CustomerName is required.")]
        [StringLength(100, ErrorMessage = "CustomerName cannot be longer than 100 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "OrderItems are required.")]
        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemDTO> OrderItems { get; set; }
    }
    public class OrderItemDTO
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [JsonIgnore]
        public ProductDto? Product { get; set; }
        public string? OrderItemId { get; set; }
        public string? OrderId {  get; set; }   
    }
}
