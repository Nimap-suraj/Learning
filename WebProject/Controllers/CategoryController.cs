using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProjectWithDto.Data;
using WebApiProjectWithDto.Dto;
using WebProject.Data;
using WebProject.Dto;
using WebProject.Services;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryServices;
        public CategoryController(ICategoryInterface categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto category)
        {
            var Item = await _categoryServices.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategoryById),new Category { Name = category.Name },Item);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var Item = await _categoryServices.GetCategories();
            return Ok(Item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var item = await _categoryServices.GetCategoryById(id);
            if (item == null) return null;
            return Ok(item);
        }

  

    }
}
