namespace yalms.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using yalms.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<yalms.Models.EFContext>
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
            var userManager = new Microsoft.AspNet.Identity.UserManager<DomainUser>(
                                  new Microsoft.AspNet.Identity.EntityFramework.UserStore<DomainUser>(ctx));
            foreach (var username in usernames)
            {
                var email = username + "@edu.com";
                if (userManager.FindByName(email) != null)
                    continue;
                var user = new DomainUser { UserName = email, Email = email };
                userManager.Create<DomainUser, string>(user, "1Hemlighet!");
                if (username.StartsWith("teacher"))
                    userManager.AddToRole(user.Id, "teacher");
                else if (username.StartsWith("student"))
                    userManager.AddToRole(user.Id, "student");
            }
        }


        protected override void Seed(yalms.Models.EFContext ctx)
        {
            seedRoles(ctx);
            seedUsers(ctx);
        }
    }
}
