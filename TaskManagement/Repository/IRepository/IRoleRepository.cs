using Microsoft.AspNetCore.Identity;
using TaskManagement.Models.DTO.Role;

namespace TaskManagement.Repository.IRepository
{
    public interface IRoleRepository
    {
        Task AddRole(IdentityRole entity);

        Task<List<IdentityRole>> GetAllAsync();

        Task AssignClaimAsync(AssignClaimRequest assignClaimRequest);
    }
}
