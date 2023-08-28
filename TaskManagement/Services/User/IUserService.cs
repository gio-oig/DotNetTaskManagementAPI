using Microsoft.AspNetCore.Identity;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.User;

namespace TaskManagement.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDTO>> Add(CreateUserRequestDto newUser);
        Task<ServiceResponse<LoginReponseDTO>> login(LogInRequestDTO logInRequestDTO);
    }
}
