using Microsoft.EntityFrameworkCore;
using WebApiProjectWithDto.Data;
using WebApiProjectWithDto.Dto;

namespace WebApiProjectWithDto.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> CreateUser(CreateUserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                
            };
            //throw new NotImplementedException();
        }



        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Manual mapping: List<User> → List<UserDTO>
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();
        }


        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
