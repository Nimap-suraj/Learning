using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.Models;
using System.Linq;
using Mapster;
using TaskEcommerce.DTO;

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
        public async Task<ActionResult> GetOrders(int currentPage = 1, int pageSize = 10)
        {
            try
            {
                int totalCount = await _context.Orders.CountAsync();
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var orders = await _context.Orders
                    .Include(o => o.Items)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var orderDtos = orders.Adapt<List<OrderDto>>();

                var response = new
                {
                    ALLOrders = orderDtos
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while fetching orders.");
            }
        }

        // Get order by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                var response = order.Adapt<OrderDto>();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while fetching the order.");
            }
        }

        // Get orders by UserId
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found for the given user.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while fetching orders for the user.");
            }
        }

        // Create a new order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            try
            {
                // Step 1: Check if the ModelState is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);  // Return a bad request with validation errors
                }

                // Step 2: Ensure the User exists (use UserId if the order requires a user)
                var user = await _context.Users.FindAsync(order.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");  // Return 404 if no such user exists
                }

                // Step 3: Set the OrderDate to the current date
                order.OrderDate = DateTime.Now;

                // Step 4: Add the order to the database
                _context.Orders.Add(order);

                // Step 5: Save the changes to the database
                await _context.SaveChangesAsync();

                // Step 6: Return the created order with its ID (CreatedAtAction creates a response with 201 status)
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while creating the order.");
            }
        }
    }
}
