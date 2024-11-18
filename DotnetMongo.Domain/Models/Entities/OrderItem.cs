using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DotnetMongo.Domain.Models.Entities
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        // Storing reference to Product (ProductId)
        [BsonElement("productId")]
        public string ProductId { get; set; } // Reference to Product document

        [BsonElement("orderId")]
        public string OrderId { get; set; } // Reference to Order document
    }
}
