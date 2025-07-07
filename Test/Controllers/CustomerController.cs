using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Model;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IService _service;
        public CustomerController(IService service)
        {
            _service = service;
        }


        [HttpPost]
        public IActionResult add(Customer c)
        {
            _service.AddCustomer(c);
            return Ok(c);
        }
    }
}
