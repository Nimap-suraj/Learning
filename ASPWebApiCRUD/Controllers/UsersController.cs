using ASPWebApiCRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPWebApiCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly UserContext userContext;
        public UsersController(UserContext usercontext)
        {
            this.userContext = usercontext;
        }
        [HttpGet]
        [Route("GetUsers")]
        public List<Users> GetUsers()
        {

            return userContext.Users.ToList();
        }

        [HttpGet]
        [Route("GetUserById")]
        public Users GetUserByid(int id)
        {
            return userContext.Users.Where(x => x.Id == id).FirstOrDefault();
        }
           
        [HttpPost]
        [Route("AddUser")]
        public string AddUser(Users user)
        {
            string response = string.Empty;

            try
            {
                userContext.Users.Add(user);
                userContext.SaveChanges();
                response = "User Added Successfully";
            }
            catch (Exception ex)
            {
                response = "Error: " + ex.Message;
            }

            return response;
        }

        [HttpPut]
        [Route("UpdateUser")]


        public string UpdateUser(Users user)
        {

            userContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            userContext.SaveChanges();
            return "User Updated Success!!!";
        }



        [HttpDelete]
        [Route("DeleteUser")]
        public string DeleteUser(int id)
        {
            Users user = userContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if(user != null)
            {
                userContext.Users.Remove(user);
                userContext.SaveChanges();
                return "User Deleted Success!!!";
            }
            else
            {
                return "No User Found!!";
            }
        }



    }
}
