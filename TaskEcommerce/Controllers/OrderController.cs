using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.Models;
using System.Linq;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        // Get all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)  // Optionally include order items
                .ToListAsync();

            return Ok(orders);
        }

        // Get order by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)  // Optionally include order items
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();  // Return 404 if not found
            }

            return Ok(order);
        }

        // Get orders by UserId
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)  // Optionally include order items
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();  // Return 404 if no orders for the user
            }

            return Ok(orders);  // Return orders for the user
        }

        // Create a new order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }
    }
}
