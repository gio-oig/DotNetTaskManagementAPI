using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.Role;
using TaskManagement.Repository.IRepository;

namespace TaskManagement.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ServiceResponse<string>> Create(string roleName)
        {
            ServiceResponse<string> _respone = new();

            var role = new IdentityRole(roleName);

            try
            {
                await _roleRepository.AddRole(role);

                _respone.Message = "Role has been created";
            } catch (Exception ex)
            {
                _respone.Error = "Could not create role";
                _respone.Success = false;
                _respone.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _respone;
        }

        public async Task<ServiceResponse<List<IdentityRole>>> GetAll()
        {
            ServiceResponse<List<IdentityRole>> _respone = new();

            try
            {
                var roles = await _roleRepository.GetAllAsync();
                _respone.Data = roles;
            }
            catch (Exception ex)
            {
                _respone.Error = "Could not get roles";
                _respone.Success = false;
                _respone.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _respone;
        }

        public async Task<ServiceResponse<string>> AssignClaim(AssignClaimRequest assignClaimRequest)
        {
            ServiceResponse<string> _respone = new();

            try
            {
                await _roleRepository.AssignClaimAsync(assignClaimRequest);
                _respone.Data = "Claim assigned successfully";
            }
            catch (Exception ex)
            {
                _respone.Error = "Could not assign claim to a role";
                _respone.Success = false;
                _respone.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _respone;
        }
    }
}
