using Mapster;
using Microsoft.AspNetCore.Mvc;
using TaskEcommerce.DTO;
using TaskEcommerce.Models;
using TaskEcommerce.Services.Interface;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(int CurrentPage = 1, int PageSize = 10)
        {
            var categories = await _categoryServices.GetCategoriesAsync(CurrentPage, PageSize);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryServices.GetCategoriesByIdAsync(id);
            if (category == null)
                return NotFound("Category not found.");
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");

                if (category.Name?.ToLower() == "string" || category.CategoryNumber?.ToLower() == "string")
                    return BadRequest("Please provide valid values for Name and Category Number.");

                var result = await _categoryServices.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = result.CategoryId }, result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while adding the category.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] Category updated)
        {
            var result = await _categoryServices.UpdateCategoriesAsync(id, updated);
            if (result == null)
                return NotFound("Category not found.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryServices.SafeDeleteCategory(id);
            if (!deleted)
                return NotFound("Category not found.");
            return Ok("Category deleted successfully.");
        }
    }
}
