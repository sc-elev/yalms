namespace yalms.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using yalms.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<yalms.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        private void seedRoles(DbContext ctx)
        {
            var roleManager = 
                new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            foreach (var role in new string[] { "teacher", "student" })
            {
                var identityRole = new IdentityRole(role);
                if (roleManager.RoleExists(identityRole.Name)) 
                    continue;
                roleManager.Create(identityRole);
            }
        }
        

        private void seedUsers(DbContext ctx)
        {
            string[] usernames = new string[] { 
                "teacher1", "teacher2",
                "student1", "student2", "student3", 
                "user1", "user2" 
            };
            var userManager = new UserManager<ApplicationUser>(
                                  new UserStore<ApplicationUser>(ctx));
            foreach (var username in usernames)
            {
                var email = username + "@edu.com";
                if (userManager.FindByName(email) != null) 
                    continue;
                var user = new ApplicationUser() { UserName = email, Email = email };
                userManager.Create(user, "1Hemlighet!");
                if (username.StartsWith("teacher"))
                    userManager.AddToRole(user.Id, "teacher");
                else if (username.StartsWith("student"))
                    userManager.AddToRole(user.Id, "student");
            }
        }


        protected override void Seed(yalms.Models.ApplicationDbContext ctx)
        {
            seedRoles(ctx);
            seedUsers(ctx);
        }
    }
}
