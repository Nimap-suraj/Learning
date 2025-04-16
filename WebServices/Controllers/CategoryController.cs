using System.Globalization;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServices.Context;
using WebServices.DTO;

using WebServices.Model;

namespace WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            context = _context;
        }
        [HttpGet]
        public ActionResult<CategoryDTO> Get()
        {
            //var category = _context.categories.ToList();
            //var response = category.Adapt<List<CategoryDTO>>();
            ////return Ok(response);
            return Ok(_context.categories.ToList().Adapt<List<CategoryDTO>>());
        }
        [HttpGet("{id}")]
        public ActionResult<CategoryDTO> GetById(int id)
        {
            //var category = _context.categories.ToList();
            //var response = category.Adapt<List<CategoryDTO>>();
            ////return Ok(response);
            return Ok(_context.categories.Find(id).Adapt<CategoryDTO>());
        }


        [HttpPost]
        public string PostData(Category category)
        {

            _context.categories.Add(category); // Use correct DbSet name (CategoryDATA)
            _context.SaveChanges();

            return "Category created successfully";
        }

    }
}
