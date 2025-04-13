using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.Models;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        public readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int CurrentPage = 1,int PageSize= 10)
        {
            var TotalCount = _context.products.Count();

            var TotalPages =  (int)Math.Ceiling((decimal)TotalCount / PageSize);

            var ProductPerPage = await _context.products.Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .Include(c => c.Category)
                .ToListAsync();

            return Ok(ProductPerPage);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(int id)
        {
            var product = await _context.products
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound("not Found!");
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new {id = product.ProductId},product);
        }
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id,Product UpdatedProduct)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();  
            }
            product.ProductId = UpdatedProduct.ProductId;
            product.ProductName = UpdatedProduct.ProductName;
            product.ProductPrice = UpdatedProduct.ProductPrice;
            product.ProductStock = UpdatedProduct.ProductStock;
            product.CategoryId = UpdatedProduct.CategoryId;
            await _context.SaveChangesAsync();
            return Ok("Product UPDATED Successfully!!");
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Product Remove Success!!");
        }
    }
}
