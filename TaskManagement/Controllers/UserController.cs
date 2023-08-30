using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTO.User;
using TaskManagement.Services.User;

namespace TaskManagement.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateUserRequestDto newUser)
        {
            var res = await _userService.Add(newUser);
           
            if(res.Success)
            {
                return Ok(res);
            } 

            return BadRequest(res);
        }


        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LogInRequestDTO loginRequestData)
        {
            var res = await _userService.login(loginRequestData);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }
    }
}
