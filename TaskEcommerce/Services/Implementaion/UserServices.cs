using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.DTO;
using TaskEcommerce.Models;
using TaskEcommerce.Services.Interface;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskEcommerce.Services.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _context;

        public UserServices(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int currentPage = 1, int pageSize = 10)
        {
            var users = await _context.Users
                .Where(u => !u.isDeleted)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Adapt<List<UserDTO>>();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.isDeleted)
                return null;

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> UpdateUserAsync(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;
            if (string.IsNullOrWhiteSpace(updatedUser.Name) || updatedUser.Name.ToLower() == "string" ||
string.IsNullOrWhiteSpace(updatedUser.Address) || updatedUser.Address.ToLower() == "string")
            {
                return null;
            }

            user.Name = updatedUser.Name;
            user.Address = updatedUser.Address;
            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;


            user.isDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
