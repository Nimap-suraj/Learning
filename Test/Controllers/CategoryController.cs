using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Model;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IService _service;
        public CategoryController(IService service)
        {
            _service = service;   
        }
        [HttpPost]
        public IActionResult add(Category category)
        {
            _service.AddCategory(category);
            return Ok(category);
        }
    }
}
