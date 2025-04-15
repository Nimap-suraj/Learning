using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.DTO;
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
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int CurrentPage = 1, int PageSize = 10)
        {
            try
            {
                var TotalCount = _context.products.Count();
                var TotalPages = (int)Math.Ceiling((decimal)TotalCount / PageSize);

                var ProductPerPage = await _context.products
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .Include(c => c.Category)
                    .ToListAsync();

                return Ok(ProductPerPage);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while fetching products.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var response = product.Adapt<ProductDto>();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while fetching the product.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (product.ProductName == null || product.ProductName == "string")
                {
                    return BadRequest("Please provide valid values for Name.");
                }

                await _context.products.AddAsync(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while adding the product.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product UpdatedProduct)
        {
            try
            {
                var product = await _context.products.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (UpdatedProduct.ProductName == null || UpdatedProduct.ProductName == "string")
                {
                    return BadRequest("Please provide valid values for Name.");
                }

                product.ProductName = UpdatedProduct.ProductName;
                product.ProductPrice = UpdatedProduct.ProductPrice;
                product.ProductStock = UpdatedProduct.ProductStock;
                product.CategoryId = UpdatedProduct.CategoryId;

                await _context.SaveChangesAsync();
                return Ok("Product updated successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.products.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                _context.products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Product removed successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the product.");
            }
        }
    }
}
