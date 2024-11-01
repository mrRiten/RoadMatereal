using Microsoft.AspNetCore.Identity;
using RoadMatereal.Models;

namespace RoadMatereal.Helpers
{
    public static class RoleInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            string[] roleNames = { "Admin", "User", "Manager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new Role() { Name = roleName });
                }
            }
        }
    }
}
