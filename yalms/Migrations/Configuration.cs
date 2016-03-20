namespace yalms.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
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

        private void seedRoles(EFContext ctx)
        {
            var options = new IdentityFactoryOptions<ApplicationRoleManager>();
            options.DataProtectionProvider = null;
            options.Provider = null;
            var roleManager = ApplicationRoleManager.CreateFromDb(options, ctx);
           
            foreach (var role in new string[] { "teacher", "student" })
            {
                var customRole  = new CustomRole(role);
                if (roleManager.RoleExists(customRole.Name))
                    continue;
                roleManager.Create(customRole);
            }
        }


        private void seedUsers(DbContext ctx)
        {
            string[] usernames = new string[] { 
                "teacher1", "teacher2",
                "student1", "student2", "student3", 
                "user1", "user2" 
            };
            var options = new IdentityFactoryOptions<ApplicationUserManager>();
            options.DataProtectionProvider = null;
            options.Provider = null;
            var userManager = ApplicationUserManager.CreateFromDb(options, new EFContext());
          
            foreach (var username in usernames)
            {
                var email = username + "@edu.com";
                if (userManager.FindByName(email) != null)
                    continue;
                var user = new DomainUser { UserName = email, Email = email };
                userManager.Create<DomainUser, int>(user, "1Hemlighet!");
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
