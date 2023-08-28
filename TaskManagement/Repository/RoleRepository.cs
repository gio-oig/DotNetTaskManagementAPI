using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using TaskManagement.Data;
using TaskManagement.Models.DTO.Role;
using TaskManagement.Repository.IRepository;

namespace TaskManagement.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        RoleManager<IdentityRole> _roleManager;

        public RoleRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleMnager)
        {
            _context = context;
            _roleManager = roleMnager;
        }
        public async Task AddRole(IdentityRole entity)
        {
             await _roleManager.CreateAsync(entity);
        }


        public async Task<List<IdentityRole>> GetAllAsync()
        {
            var roles = _roleManager.Roles;

            return await roles.ToListAsync();

        }

        public async Task AssignClaimAsync(AssignClaimRequest assignClaimRequest)
        {
            var role = await _roleManager.FindByIdAsync(assignClaimRequest.RoleId);

            if (role is null)
            {
                throw new Exception("role does not exists");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claimValue in assignClaimRequest.Permissions)
            {
                var existingClaim = roleClaims.FirstOrDefault(c => c.Type == "Permission" && c.Value == claimValue);
                if (existingClaim is not null) throw new Exception("Role has the specified claim.");
                
                var claim = new Claim("Permission", claimValue);

                var result = await _roleManager.AddClaimAsync(role, claim);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to assign claims to role.");
                }
            }

        }
    }
}
