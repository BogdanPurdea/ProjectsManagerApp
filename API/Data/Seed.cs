using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            if(users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
        }

        public static async Task SeedProjects(DataContext context)
        {
            if (await context.Projects.AnyAsync()) return;

            var projectData = await System.IO.File.ReadAllTextAsync("Data/ProjectSeedData.json");
            var projects = JsonSerializer.Deserialize<List<Project>>(projectData);
            if (projects == null) return;

            foreach (var project in projects)
            {
                project.Creator!.UserName = project.Creator.UserName.ToLower();
                project.Creator = context.Users.FirstOrDefault(user => user.UserName == project.Creator.UserName)!;
                var updatedContributors = new List<AppUser>();
                foreach(var contributor in project.Contributors!)
                {
                    contributor.UserName = contributor.UserName.ToLower();
                    updatedContributors.Add(context.Users.FirstOrDefault(user => user.UserName == contributor.UserName)!);
                }
                project.Contributors = updatedContributors;
                context.Projects.Add(project);
            }

            await context.SaveChangesAsync();
        }
    }
}