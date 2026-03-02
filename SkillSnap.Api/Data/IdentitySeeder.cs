using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRoles(IServiceProvider services)
    {
        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "User" };

        foreach(var role in roles)
        {
            if(!await rolemanager.RoleExistsAsync(role))
            {
                await rolemanager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}