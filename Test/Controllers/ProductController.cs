using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Model;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IService _service;
        public ProductController(IService service)
        {
            _service = service;
        }
       

        [HttpPost]
        public IActionResult add(Product product)
        {
            _service.AddProduct(product);
            var g=_service.GetProductWithMaxOrder();
            return Ok(new
            {
                product,g.Result
            }); 
        }
        [HttpGet("max-ordered-product")]
        public async Task<IActionResult> GetProductWithMaxOrder()
        {
            var data = await _service.GetProductWithMaxOrder();

            //if (data == null)
            //{
            //    return NotFound("No product order data found.");
            //}

            return Ok(data); // returns entire Product object
        }

        //[HttpGet]
        //public IActionResult get(Product product)
        //{
        //    _service.getProduct(product);
        //    return Ok(product);
        //}
    }
}
