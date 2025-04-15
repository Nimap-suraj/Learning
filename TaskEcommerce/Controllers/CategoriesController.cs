using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.DTO;
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
        public async Task<ActionResult<IEnumerable<Category>>> GetALLCategories(int CurrentPage = 1,int PageSize = 10)
        {
            int TotalCategory = _context.categories.Count();
            int TotalPages = (int)Math.Ceiling((decimal)TotalCategory / PageSize);

            var TotalCategories = await _context.categories.Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            return Ok(TotalCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            try
            {
                var category = await _context.categories.FindAsync(id);

                if (category == null || category.Name == "string")
                    return NotFound();
                var response = category.Adapt<CategoryDto>();

                return Ok(response);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something Went Wrong!,Please Try Again Later!!!");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");

                if (category.Name?.ToLower() == "string" || category.CategoryNumber?.ToLower() == "string")
                    return BadRequest("Please provide valid values for Name and Category Number.");

                _context.categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while adding the category.");
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateCategory(int id,[FromBody]Category updated)
        {
            try
            {

                var category = await _context.categories.FindAsync(id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // 2. Custom logic validation to avoid default/placeholder values
                if (updated.Name?.ToLower() == "string" || updated.CategoryNumber?.ToLower() == "string")
                {
                    return BadRequest("Please provide valid values for Name and Category Number.");
                }

                if (category == null) 
                    return BadRequest("Data Not Found!");

                category.Name = updated.Name;
                category.CategoryNumber = updated.CategoryNumber;
                await _context.SaveChangesAsync();
                return Ok("Update Done");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while adding the category.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _context.categories.FindAsync(id);
                if (category == null)
                    return NotFound();

                _context.categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok("Data Deleted!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while adding the category.");
            }
        }


    }
}
