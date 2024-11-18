using DotnetMongo.Application.Contracts.DTOs;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotnetMongo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var orders = await _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var order = await _orderService.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                // Check for date errors manually
                var orderDate = orderDto.OrderDate;
                if (orderDate == null || !DateTime.TryParse(orderDate.ToString(), out _))
                {
                    ModelState.AddModelError("orderDate", "Invalid date format.");
                }
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrder = await _orderService.AddOrder(orderDto);

                // Return the created order with a 201 Created status
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, OrderDto orderDto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrder(id, orderDto);
                if (updatedOrder == null)
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
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
