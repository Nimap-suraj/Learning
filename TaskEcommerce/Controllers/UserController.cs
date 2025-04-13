using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.Models;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserById(int id)
        {
            var product = await _context.Users.FindAsync(id);
            if (product == null)
            {
                return NotFound("not Found!");
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, User UpdatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            //if(category == null) return BadRequest();
            user.Name = UpdatedUser.Name;
            user.Address = UpdatedUser.Address;
            await _context.SaveChangesAsync();
            return Ok("Update Done");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            user.isDeleted = true;

            //_context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User Removed! SAFE DELETE!!!");
        }
        

    }
}
