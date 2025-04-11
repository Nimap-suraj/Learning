using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlRelationship.Data;
using SqlRelationship.Entities;

namespace SqlRelationship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _context.User
                .Include(u => u.Address)
                .Include(u => u.Products)
                .Include(u => u.Coupons)
                .ToListAsync();

            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserById(int id)
        {
            var user = await _context.User
                .Include(u=> u.Address)
                .Include(u => u.Products)
                .Include(u => u.Coupons)
                .FirstOrDefaultAsync(u => u.Id == id);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

    }
}
