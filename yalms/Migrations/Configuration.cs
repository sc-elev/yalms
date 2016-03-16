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
            string[] usernames =
                 new string[] { "teacher1", "student1", "student2", "student3" };
            var userManager = new UserManager<ApplicationUser>(
                                  new UserStore<ApplicationUser>(ctx));
            foreach (var username in usernames)
            {
                var email = username + "@edu.com";
                if (userManager.FindByName(email) != null) 
                    continue;
                var user = new ApplicationUser() { UserName = email };
                var role = username.StartsWith("teacher") ? "teacher" : "student";
                userManager.Create(user, "1Hemlighet!");
                userManager.AddToRole(user.Id, role);
            }
        }


        protected override void Seed(yalms.Models.ApplicationDbContext ctx)
        {
            seedRoles(ctx);
            seedUsers(ctx);
        }
    }
}
