using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.Models;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetALLCategories()
        {
            return await _context.categories.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            _context.categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId}, category);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateCategory(int id,Category updated)
        {
           var category = await _context.categories.FindAsync(id);
            //if(category == null) return BadRequest();
            category.Name = updated.Name;
            category.CategoryNumber = updated.CategoryNumber;
            await _context.SaveChangesAsync();
            return Ok("Update Done");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
