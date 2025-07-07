using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Test.Model;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IService _service;
        public OrderController(IService service)
        {
            _service = service;
        }
        [HttpPost]  
        public IActionResult add(Order o)
        {
            _service.AddOrder(o);
            return Ok(o);
        }

       

    }
}
