using Microsoft.AspNetCore.Mvc;

namespace AnonymousMethod.Controllers
{
    [ApiController]
    [Route("Test/[action]")]
    public class TestController : ControllerBase
    {
        public string Get()
        {
            return "Hello from GET";
        }
        public string Get1()
        {
            return "Hello from GET1";
        }
    }
}
