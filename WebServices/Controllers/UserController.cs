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
    public class UserController : ControllerBase
    {
        private readonly DataContext _Context;
        public UserController(DataContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public List<UserDTO> Get()
        {
            var users =_Context.Users.ToList();
            var data = users.Adapt<List<UserDTO>>();
            return data;
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
           var user = _Context.Users.FirstOrDefault(u => u.Id == id);
           if(user == null)
            {
                return BadRequest("No User Found!");
            }
           return user;
        }
        [HttpPost]
        public string PostData([FromBody]UserDTO user)
        {
            var data = new User 
            { 
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            };   
            if(data.UserName.ToLower() == "string")
            {
                return "Something went Wrong!!";
            }
            _Context.Users.Add(data);
            _Context.SaveChanges();
            return "Data Created";
        }
        [HttpPut]
        public ActionResult<User> PutData(int id, [FromBody] UserDTO newUser)
        {

            var user = _Context.Users.Find(id);
           
            if (user == null)
            {
                return BadRequest("No User Found!");
            }
            user.UserName = newUser.UserName;
            user.Password = newUser.Password;
            user.Email = newUser.Email;
            _Context.SaveChanges();
            return user;
        }
        [HttpDelete]
        public ActionResult<String> DeleteDate(int id)
        {

            var user = _Context.Users.Find(id);
            if (user == null)
            {
                return BadRequest("No User Found!");
            }
            _Context.Users.Remove(user);
            _Context.SaveChanges();
            return "user is removed!";
        }
    }
}
