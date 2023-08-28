using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TaskManagement.Data
{
    public static class Seeder
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "AdminPassword123!";

            string[] roles = { "Admin", "Manager", "User" };
            string[] permissions = { "Task_Create", "Task_Update", "Task_Delete" };

            // Create roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create an admin user
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                // Assign the "Admin" role to the admin user
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var adminRole = await roleManager.FindByNameAsync("Admin");

                if(adminRole == null)
                {
                    throw new Exception("Could not find an Admin role");
                }

                // Assign permissions to roles (Task_Create, Task_Update, Task_Delete)
                foreach (var permission  in permissions)
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                }
                //await roleManager.AddClaimAsync(await roleManager.FindByNameAsync("Admin"), new Claim("Permission", "Task_Create"));
                //await roleManager.AddClaimAsync(await roleManager.FindByNameAsync("Admin"), new Claim("Permission", "Task_Update"));
                //await roleManager.AddClaimAsync(await roleManager.FindByNameAsync("Admin"), new Claim("Permission", "Task_Delete"));
            }
        }
    }
}
