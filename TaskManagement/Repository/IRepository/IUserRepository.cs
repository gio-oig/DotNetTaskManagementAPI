using TaskManagement.Models;
using TaskManagement.Models.DTO.User;

namespace TaskManagement.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> Add(CreateUserRequestDto newUser);
        Task<LoginReponseDTO> LogIn(LogInRequestDTO loginDto);
    }
}
