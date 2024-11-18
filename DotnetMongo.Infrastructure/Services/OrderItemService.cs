using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Application.Services
{
    public class OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository) : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Order> GetOrderItemsByOrderId(string orderId)
        {
            // Get the order by orderId
            var order = await _orderRepository.GetOrderById(orderId);

            // If no order found, return null
            if (order == null)
            {
                return null; 
            }

            // Get order items based on orderItemIds
            var orderItems = await _orderItemRepository.GetOrderItemsByIds(order.OrderItemIds);

            // Assuming that OrderItems are stored as references (list of Ids)
            order.OrderItems = orderItems; // Assuming Order has a collection property for OrderItems

            return order;
        }
        public async Task<OrderItem> AddOrderItem(OrderItemDTO orderItemDto)
        {
            // Create a new OrderItem entity based on the provided OrderItemDTO
            var orderItem = new OrderItem
            {
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity,
                OrderId = orderItemDto.OrderId // Assuming OrderItemDTO has OrderId
            };

            // Insert the order item into the database
            await _orderItemRepository.AddOrderItem(orderItem);

            // Retrieve the associated order by OrderId
            var order = await _orderRepository.GetOrderById(orderItemDto.OrderId);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            // Add the created OrderItem's ID to the Order's OrderItemIds list
            order.OrderItemIds.Add(orderItem.Id);

            // Update the Order to include the new OrderItem reference
            await _orderRepository.UpdateOrder(order);

            return orderItem;
        }
        public async Task<OrderItem> UpdateOrderItem(string id, OrderItemDTO orderItemDto)
        {
            // Retrieve the existing OrderItem by its ID
            var existingOrderItem = await _orderItemRepository.GetOrderItemById(id) ?? throw new Exception("Order item not found");

            // Update the OrderItem's fields based on the provided DTO
            existingOrderItem.Quantity = orderItemDto.Quantity;
            existingOrderItem.ProductId = orderItemDto.ProductId;

            // Save the updated order item
            await _orderItemRepository.UpdateOrderItem(existingOrderItem);

            return existingOrderItem;
        }
        public async Task DeleteOrderItemAndUpdateOrder(string orderItemId)
        {
            // Retrieve the order item to get the associated OrderId
            var orderItem = await _orderItemRepository.GetOrderItemById(orderItemId);

            if (orderItem == null)
            {
                throw new Exception("Order item not found.");
            }

            // Delete the order item from the OrderItems collection
            await _orderItemRepository.DeleteOrderItem(orderItemId);

            // Update the corresponding order to remove the reference to the deleted order item
            await _orderRepository.RemoveOrderItemReference(orderItem.OrderId, orderItemId);
        }
    }
}
