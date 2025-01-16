using Microsoft.AspNetCore.Identity;
using todo_webapi.Models;

namespace todo_webapi.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

       
            var user = await userManager.FindByEmailAsync("test@example.com");
            if (user == null)
            {
                
                var newUser = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "test@example.com"
                };

               
                var result = await userManager.CreateAsync(newUser, "my_secure_password2345AA$");

                if (!result.Succeeded)
                {
                    throw new Exception("Помилка при створенні користувача: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
