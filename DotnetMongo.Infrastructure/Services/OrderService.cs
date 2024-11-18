using AutoMapper;
using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Domain.Models.Entities;

namespace DotnetMongo.Infrastructure.Services
{
    public class OrderService(IMapper mapper, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository) : IOrderService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        public async Task<IEnumerable<Order>> GetAllOrders()
        { 
            // Fetch all orders
            var orders = await _orderRepository.GetAllOrders();

            // Populate OrderItems for each order
            foreach (var order in orders)
            {
                if (order.OrderItemIds != null && order.OrderItemIds.Any())
                {
                    // Fetch OrderItems based on OrderItemIds
                    var orderItems = await _orderItemRepository.GetOrderItemsByIds(order.OrderItemIds);
                    order.OrderItems = orderItems.ToList();
                }
            }

            return orders;
        }
        public async Task<Order> GetOrderById(string id)
        {
            var order = await _orderRepository.GetOrderById(id);
            return order;
        }
        public async Task<Order> AddOrder(OrderDto orderDto)
        {
            // Ensure that the orderDto is not null
            ArgumentNullException.ThrowIfNull(orderDto);

            orderDto.OrderDate = orderDto.OrderDate?.ToUniversalTime();

            // Map the DTO to the entity using AutoMapper
            var order = _mapper.Map<Order>(orderDto);

            // Add the mapped order to the database
            var createdOrder = await _orderRepository.AddOrder(order);

            // Save each OrderItem and get their generated IDs
            var orderItemIds = new List<string>();

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                // Map DTO to entity
                var orderItem = _mapper.Map<OrderItem>(orderItemDto);

                // Link the OrderItem to the created Order by setting the orderId
                orderItem.OrderId = createdOrder.Id;

                // Save the order item to the database
                await _orderItemRepository.AddOrderItem(orderItem);

                // Add the generated OrderItem ID
                orderItemIds.Add(orderItem.Id); 
            }

            // After saving the OrderItems, update the Order with their IDs
            createdOrder.OrderItemIds = orderItemIds;

            // Save the updated Order with the OrderItemIds back to the database
            await _orderRepository.UpdateOrder(createdOrder);

            // Explicitly load related OrderItems and Products using GetOrderById
            var orderWithProducts = await _orderRepository.GetOrderById(createdOrder.Id);

            // Return the order with the loaded product details
            return orderWithProducts;
        }
        public async Task<Order> UpdateOrder(string id, OrderDto orderDto)
        {
            // Convert OrderDate to UTC
            if (orderDto.OrderDate.HasValue)
            {
                orderDto.OrderDate = DateTime.SpecifyKind(orderDto.OrderDate.Value, DateTimeKind.Utc);
            }

            // Fetch the existing order from the repository
            var existingOrder = await _orderRepository.GetOrderById(id);

            if (existingOrder == null)
            {
                return null;
            }

            // Map the incoming orderDto to the existing order
            _mapper.Map(orderDto, existingOrder);

            // Clear the existing OrderItemIds to update them with new ones
            existingOrder.OrderItemIds.Clear();

            // Save each OrderItem and collect their IDs
            var orderItemIds = new List<string>();

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                // Check if the OrderItem exists
                var existingOrderItem = await _orderItemRepository.GetOrderItemById(orderItemDto.OrderItemId);

                if (existingOrderItem != null)
                {
                    // Update the existing OrderItem
                    existingOrderItem.Quantity = orderItemDto.Quantity;
                    existingOrderItem.ProductId = orderItemDto.ProductId;

                    // Save the updated OrderItem (this could be an update operation instead of insert)
                    await _orderItemRepository.UpdateOrderItem(existingOrderItem);

                    // Add the existing OrderItem ID to the list
                    orderItemIds.Add(existingOrderItem.Id);
                }
                else
                {
                    // Map DTO to OrderItem entity for new items
                    var newOrderItem = _mapper.Map<OrderItem>(orderItemDto);
                    newOrderItem.OrderId = existingOrder.Id;  // Set the OrderId

                    // Save the new OrderItem to the database
                    await _orderItemRepository.AddOrderItem(newOrderItem);

                    // Add the generated OrderItem ID to the list
                    orderItemIds.Add(newOrderItem.Id);
                }
            }

            // Update the Order with the new OrderItemIds
            existingOrder.OrderItemIds = orderItemIds;

            // Update the order in the database
            await _orderRepository.UpdateOrder(existingOrder);

            return existingOrder;
        }
        public async Task DeleteOrder(string orderId)
        {
            // Retrieve the order to ensure it exists
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            // Delete all associated OrderItems
            if (order.OrderItemIds != null && order.OrderItemIds.Any())
            {
                await _orderItemRepository.DeleteOrderItemsByIds(order.OrderItemIds);
            }
            // Delete the Order
            await _orderRepository.DeleteOrder(orderId);
        }
    }
}
