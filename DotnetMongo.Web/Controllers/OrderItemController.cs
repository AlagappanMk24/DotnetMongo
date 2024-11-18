using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotnetMongo.Web.Controllers
{
    [Route("api/order-item")]
    [ApiController]
    public class OrderItemController(IOrderItemService orderItemService) : ControllerBase
    {
        private readonly IOrderItemService _orderItemService = orderItemService;

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderItemsByOrderId(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var order = await _orderItemService.GetOrderItemsByOrderId(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrderItem(OrderItemDTO orderItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrderItem = await _orderItemService.AddOrderItem(orderItemDto);

                // Return the created order with a 201 Created status
                return CreatedAtAction(nameof(GetOrderItemsByOrderId), new { id = createdOrderItem.Id }, createdOrderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(string id, OrderItemDTO orderItemDto)
        {
            try
            {
                var updatedOrderItem = await _orderItemService.UpdateOrderItem(id, orderItemDto);
                if (updatedOrderItem == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(string id)
        {
            try
            {
                await _orderItemService.DeleteOrderItemAndUpdateOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
