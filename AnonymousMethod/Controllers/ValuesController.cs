using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousMethod.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("~/")]
        public string server()
        {
            return "server is running";
        }
    
        //[Route("api/get-all")]
        //[Route("getall")]
        //[Route("get-all")]
        //token replacement
        [Route("[controller]/[action]")]
        public string GetALL()
        {
            return "hello from get All";
        }
        //[Route("[controller]/[action]")]
        //[Route("api/get-all-authors")]
        public string GetALLAuthors()
        {
            return "hello from get All authors";
        }
        //[Route("api/books/{id}/authors/{authorId}")]
        [Route("{id}")]
        public string GetAuthorByID(int id,int authorId)
        {
            return $"hey Id of book is {id} and author id is {authorId}";
        } 
        //[Route("search")]
        public string SearchBooks(int id, int authorId,string name,int rating,int price)
        {
            return "hey";
        }
    }
}
