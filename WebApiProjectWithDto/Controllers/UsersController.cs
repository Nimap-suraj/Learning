using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProjectWithDto.Dto;
using WebApiProjectWithDto.Services;

namespace WebApiProjectWithDto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UsersController(IUserService userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(UserDto userDto)
        {
            var user = await _userServices.GetAllUsers();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userServices.GetUserById(id);
            if (user == null) return null;
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            var user = await _userServices.CreateUser(userDto);
            return CreatedAtAction(nameof(GetUserById),new {Id = user.Id},user);
        }
    }
}
