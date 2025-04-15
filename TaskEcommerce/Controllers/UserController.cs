using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.DTO;
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

        // Get users with pagination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(int CurrentPage = 1, int PageSize = 10)
        {
            try
            {
                int TotalCounts = _context.Users.Count();
                int TotalPages = (int)Math.Ceiling((decimal)TotalCounts / PageSize);

                var TotalUserPerPage = await _context.Users.Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();


                return Ok(TotalUserPerPage);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while fetching users.");
            }
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var response = user.Adapt<UserDTO>();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while fetching the user.");
            }
        }

        // Add a new user
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Custom validation to avoid default/placeholder values
                if (user.Name?.ToLower() == "string" || user.Address?.ToLower() == "string")
                {
                    return BadRequest("Please provide valid values for Name and Address.");
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while adding the user.");
            }
        }

        // Update existing user
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, User UpdatedUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Custom validation to avoid default/placeholder values
                if (UpdatedUser.Name?.ToLower() == "string" || UpdatedUser.Address?.ToLower() == "string")
                {
                    return BadRequest("Please provide valid values for Name and Address.");
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.Name = UpdatedUser.Name;
                user.Address = UpdatedUser.Address;

                await _context.SaveChangesAsync();

                return Ok("User updated successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the user.");
            }
        }

        // Soft delete (mark as deleted)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.isDeleted = true;

                await _context.SaveChangesAsync();

                return Ok("User removed (soft delete).");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the user.");
            }
        }
    }
}
