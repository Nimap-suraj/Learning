using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCreate.Model;

namespace TestCreate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;   
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is null");
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("Customer created successfully");
        }

    }
}
