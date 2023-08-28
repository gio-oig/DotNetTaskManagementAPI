using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Models.DTO.User;
using TaskManagement.Repository.IRepository;

namespace TaskManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            this.secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<User> Add(CreateUserRequestDto newUser)
        {
            User user = new()
            {
                Email = newUser.Email,
                UserName = newUser.Email
            };

            try
            {
                var result = await _userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    var userToReturn = await _db.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
                    return userToReturn;
                }


                var message = String.Join(",", result.Errors.Select(op => op.Description));
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<LoginReponseDTO> LogIn(LogInRequestDTO loginDto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || !isValidPassword)
            {
                throw new Exception("userName or passowrd is incorrect");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
            };

            foreach (var role in roles)
            {
                var roleObj = await _roleManager.FindByNameAsync(role);
                if (roleObj != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleObj);
                    foreach (var claim in roleClaims)
                    {
                        claims.Add(new Claim("Permission", claim.Value));
                    }
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            var writtenToken = tokenHandler.WriteToken(token);
            var userDto = _mapper.Map<UserDTO>(user);

            return new LoginReponseDTO(userDto, writtenToken);
        }
    }
}
