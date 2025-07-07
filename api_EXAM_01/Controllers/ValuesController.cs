using api_EXAM_01.Data;
using api_EXAM_01.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace api_EXAM_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ValuesController(ApplicationDBContext context)
        {
            _context = context;
        }

      

        [HttpPost("/product")]
        public async Task<string> AddProduct(Product product)
        {
            var exit = await _context.products.FindAsync(product.Id);
            if (exit == null)
            {
                // nahi mila
                _context.products.Add(product);
            }
            else
            {
                _context.products.Update(product);
            }
            await _context.SaveChangesAsync();
            return "done";
        }


        [HttpPost("/category")]
        public async Task<string> AddCategory(Category category)
        {
            var exit = await _context.categories.FindAsync(category.Id);
            if (exit == null)
            {
                // nahi mila
                _context.categories.Add(category);
            }
            else
            {
                _context.categories.Update(category);
            }
            await _context.SaveChangesAsync();
            return "done";
        }

        [HttpPost("/customer")]
        public async Task<string> AddCustomer(Customer customer)
        {
            var exit = await _context.customers.FindAsync(customer.Id);
            if (exit == null)
            {
                // nahi mila
                _context.customers.Add(customer);   
            }
            else
            {
                _context.customers.Update(customer);
            }
            await _context.SaveChangesAsync();
            return "done";
        }
        [HttpPost("/order")]
        public async Task<string> AddOrder(Order order)
        {
            var exit = await _context.orders.FindAsync(order.Id);
            if (exit == null)
            {
                // nahi mila
                _context.orders.Add(order);
            }
            else
            {
                _context.orders.Update(order);
            }
            await _context.SaveChangesAsync();
            return "done";
        }

    }
}
