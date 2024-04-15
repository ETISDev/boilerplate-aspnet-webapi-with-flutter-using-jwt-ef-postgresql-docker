
using System.Collections.Generic;

namespace CoreApi.Data
{
    public class DataSeeder
    {
        public DataSeeder()
        {
        }
        public static void SeedSettings(CoreContext context, UserManager<UserInfo> userManager, RoleManager<Role> roleManager)
        {
            // Roles
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new Role { Name = "SuperUser" }).Wait();
                roleManager.CreateAsync(new Role { Name = "Admin" }).Wait();
                roleManager.CreateAsync(new Role { Name = "User" }).Wait();
                Console.WriteLine("#########################");
                Console.WriteLine("Roles Added");
                Console.WriteLine("#########################");
            }


            // Create Superuser User
            if (!userManager.Users.Any())
            {
                Random rnd = new Random();
                var accessCode = rnd.Next(100000, 999999);
                var superUser = new UserInfo
                {
                    AccessCode = accessCode,
                    UserName = "SuperUser",
                    FirstName = "Super",
                    LastName = "User",
                    MiddleName = "",
                    Gender = "O",
                };
                var superUserResult = userManager.CreateAsync(superUser, "Password@123").Result;
                Console.WriteLine("#########################");
                Console.WriteLine("SuperUser Created");
                foreach (var err in superUserResult.Errors)
                {
                    Console.WriteLine("*****");
                    Console.WriteLine(err.Code);
                    Console.WriteLine(err.Description);
                }
                Console.WriteLine(superUserResult.Succeeded);
                Console.WriteLine("#########################");


                if (superUserResult.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("SuperUser").Result;
                    if (admin != null)
                    {
                        userManager.AddToRolesAsync(admin, new[] { "Admin", "SuperUser" }).Wait();
                        Console.WriteLine("#########################");
                        Console.WriteLine("Role assigned to SuperUser.");
                        Console.WriteLine("#########################");
                    }
                }

            }
        }
    }
}
