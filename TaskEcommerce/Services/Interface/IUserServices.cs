using TaskEcommerce.DTO;
using TaskEcommerce.Models;

namespace TaskEcommerce.Services.Interface
{
    public interface IUserServices
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(int page, int pageSize);
        Task<UserDTO?> GetUserByIdAsync(int id);

        Task<UserDTO> CreateUserAsync(User user);

        Task<UserDTO> UpdateUserAsync(int id, User UpdatedUser);

        Task<bool> SoftDeleteUserAsync(int id);

    }
}
