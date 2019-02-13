namespace Zblog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Zblog.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Zblog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Zblog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            //Create a few users in the project

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "gzambrana93@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "gzambrana93@outlook.com",
                    Email = "gzambrana93@outlook.com",
                    FirstName = "Gregorio",
                    LastName = "Zambrana",
                    DisplayName = "Greg"
                }, "Abc&123!");

                //Associate a User to a role
            }
                var userId = userManager.FindByEmail("gzambrana93@outlook.com").Id;
                userManager.AddToRole(userId, "Admin");
            
            if (!context.Users.Any(u => u.Email == "JTwichell@mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "JTwichell@mailinator.com",
                    Email = "JTwichell@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "J"
                }, "Abc&123!");

            }   //Associate a User to a role

                userId = userManager.FindByEmail("JTwichell@mailinator.com").Id;
                userManager.AddToRole(userId, "Moderator");
            





        }

        private static string NewMethod()
        {
            return "Greg!";
        }
    }
}
