using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.User;
using TaskManagement.Repository.IRepository;

namespace TaskManagement.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepos;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepos, IMapper mapper)
        {
            _userRepos = userRepos;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<UserDTO>> Add(CreateUserRequestDto newUser)
        {
            ServiceResponse<UserDTO> _respone = new();

            try
            {
                var user = await _userRepos.Add(newUser);
                var userDto = _mapper.Map<UserDTO>(user);
                _respone.Data = userDto;

            }
            catch (Exception ex)
            {
                _respone.Error = "Could not create user";
                _respone.Success = false;
                _respone.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }


            return _respone;
        }

        public async Task<ServiceResponse<LoginReponseDTO>> login(LogInRequestDTO logInRequestDTO)
        {
            ServiceResponse<LoginReponseDTO> _respone = new();

            try
            {
                var userWithToken = await _userRepos.LogIn(logInRequestDTO);
                _respone.Data = userWithToken;
            }
            catch (Exception ex)
            {
                _respone.Error = "Something whent wrong";
                _respone.Success = false;
                _respone.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _respone;
        }
    }
}
