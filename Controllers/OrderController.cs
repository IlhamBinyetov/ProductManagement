using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.DTOs.CustomerDTOs;
using ProductManagement.DTOs.OrderDTOs;
using ProductManagement.DTOs.OrderItemDTOs;
using ProductManagement.DTOs.ProductDTOs;
using ProductManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllOrders")]
        [ProducesResponseType(typeof(SysResponse<List<Order>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders
                                       .Include(o => o.Customer)
                                       .Include(o => o.OrderItems)
                                       .ThenInclude(oi => oi.Product)
                                       .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return NotFound(SysResponse.Error(null, "No orders found"));
                }

                var orderDtos = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Customer = new CustomerDto
                    {
                        Id = o.Customer.Id,
                        Name = o.Customer.Name,
                        Email = o.Customer.Email
                    },
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            Name = oi.Product.Name,
                            Description = oi.Product.Description,
                            Price = oi.Product.Price,
                            Stock = oi.Product.Stock
                        }
                    }).ToList()
                }).ToList();

                return Ok(SysResponse.Success(orders, "Orders retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, SysResponse.Error(null, $"An error occurred: {ex.Message}"));
            }
        }



        [HttpGet("GetOrder/{id}")]
        [ProducesResponseType(typeof(SysResponse<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.Include(x=>x.Customer).Include(o => o.OrderItems).ThenInclude(y=>y.Product).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(SysResponse.Error(null, "Order not found"));
            }

            return Ok(SysResponse.Success(order));
        }


        [HttpPost("AddOrder")]
        [ProducesResponseType(typeof(SysResponse<Order>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> AddOrder([FromBody] OrderCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(SysResponse.Error(ModelState, "Invalid input"));
            }
            var order = new Order
            {
                OrderDate = createDto.OrderDate,
                CustomerId = createDto.CustomerId,
                OrderItems = createDto.OrderItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound(SysResponse.Error(null, "Order cannot created"));
            }

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }


        [HttpPut("UpdateOrder/{id}")]
        [ProducesResponseType(typeof(SysResponse<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(SysResponse.Error(ModelState, "Invalid input"));
            }

            var order = await _context.Orders.Include(o => o.OrderItems)
                                            .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound(SysResponse.Error(order, "Order not found"));
            }

            order.OrderDate = updateDto.OrderDate;
            order.CustomerId = updateDto.CustomerId;

            var existingOrderItems = order.OrderItems.ToDictionary(oi => oi.Id);

            foreach (var itemDto in updateDto.OrderItems)
            {
                if (existingOrderItems.TryGetValue(itemDto.Id, out var existingItem))
                {
                    existingItem.ProductId = itemDto.ProductId;
                    existingItem.Quantity = itemDto.Quantity;
                    existingItem.UnitPrice = itemDto.UnitPrice;
                }
                else
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice
                    });
                }
            }


            // Remove OrderItems not in the updateDto
            foreach (var existingItemId in existingOrderItems.Keys)
            {
                if (!updateDto.OrderItems.Any(oi => oi.Id == existingItemId))
                {
                    var itemToRemove = existingOrderItems[existingItemId];
                    order.OrderItems.Remove(itemToRemove);
                    _context.OrderItems.Remove(itemToRemove);
                }
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound(SysResponse.Error(null, "Order cannot be updated"));
            }

            return Ok(SysResponse.Success(order, "Order updated successfully"));
        }


        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems)
                                             .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(SysResponse.Error(null, "Order not found"));
            }

            _context.Orders.Remove(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound(SysResponse.Error(null, "Order cannot be deleted"));
            }

            return Ok(SysResponse.Success(null, "Order deleted successfully"));
        }
    }
}
