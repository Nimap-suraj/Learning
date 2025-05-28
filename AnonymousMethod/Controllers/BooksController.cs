using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousMethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        [Route("{id:int:min(1):max(10)}")]
        public string getId(int id)
        {
            return $"Hey id is {id}";
        }
        [Route("{id:minlength(2):alpha}")]
        public string GetName(string id)
        {
            return $"hey string is {id}";
        }
        [Route("{id:regex(^a$|^bc$)}")]
        public string GetRegex(string id)
        {
            return $"hey string is {id}";
        }
    }
}
