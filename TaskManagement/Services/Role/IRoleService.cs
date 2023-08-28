using Microsoft.AspNetCore.Identity;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.Role;

namespace TaskManagement.Services.Role
{
    public interface IRoleService
    {
        Task<ServiceResponse<string>> Create(string roleName);

        Task<ServiceResponse<List<IdentityRole>>> GetAll();

        Task<ServiceResponse<string>> AssignClaim(AssignClaimRequest assignClaimRequest);
    }
}
