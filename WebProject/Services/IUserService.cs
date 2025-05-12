using WebApiProjectWithDto.Dto;

namespace WebApiProjectWithDto.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(CreateUserDto userDto);
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
    }
}
