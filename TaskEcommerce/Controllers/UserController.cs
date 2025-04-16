using Microsoft.AspNetCore.Mvc;
using TaskEcommerce.DTO;
using TaskEcommerce.Models;
using TaskEcommerce.Services.Interface;

namespace TaskEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(int currentPage = 1, int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(currentPage, pageSize);
                return Ok(users);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while fetching users.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while fetching the user.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (user.Name?.ToLower() == "string" || user.Address?.ToLower() == "string")
                    return BadRequest("Please provide valid values for Name and Address.");

                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while adding the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (updatedUser.Name?.ToLower() == "string" || updatedUser.Address?.ToLower() == "string")
                    return BadRequest("Please provide valid values for Name and Address.");

                var updated = await _userService.UpdateUserAsync(id, updatedUser);
                if (updated == null)
                    return NotFound("User not found.");

                return Ok("User updated successfully!");
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var deleted = await _userService.SoftDeleteUserAsync(id);
                if (!deleted)
                    return NotFound("User not found.");

                return Ok("User soft-deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while deleting the user.");
            }
        }
    }
}
