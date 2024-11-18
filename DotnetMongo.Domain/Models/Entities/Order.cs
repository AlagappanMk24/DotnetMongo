using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DotnetMongo.Domain.Models.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("customerName")]
        public string CustomerName { get; set; }

        // Storing references to OrderItems (as ObjectIds)
        [BsonElement("orderItemIds")]
        public List<string> OrderItemIds { get; set; } // List of references to OrderItem documents

        [BsonIgnore] // Ignore this property when storing in the database
        public List<OrderItem> OrderItems { get; set; } // List of OrderItem objects
    }
}
