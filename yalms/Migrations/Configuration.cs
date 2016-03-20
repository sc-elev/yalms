namespace yalms.Migrations
{
    using Microsoft.AspNet.Identity.Owin;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;

    using yalms.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<yalms.Models.EFContext>
    {

        private void seedRoles(EFContext ctx)
        {
            var options = new IdentityFactoryOptions<ApplicationRoleManager>();
            options.DataProtectionProvider = null;
            options.Provider = null;
            var roleManager = ApplicationRoleManager.CreateFromDb(options, ctx);

            foreach (var role in new string[] { "teacher", "student" })
            {
                var customRole = new CustomRole(role);
                if (roleManager.RoleExists(customRole.Name))
                    continue;
                roleManager.Create(customRole);
            }
        }

        private void seedClasses(EFContext ctx)
        {
            SchoolClass sc = new SchoolClass() { SchoolClassID = 1, Name = "7a" };
            SchoolClassStudent scs = new SchoolClassStudent() { 
                SchoolClassID = 1, SchoolClassStudentID = 3
            };
            ctx.SchoolClasses.AddOrUpdate(sc);
            ctx.SchoolClassStudents.AddOrUpdate(scs);
            ctx.SaveChanges();;
        }

        private void seedCourseAndSchema(EFContext ctx)
        {
            Course c = new Course() { 
                Name = "Matematik A", Teacher_UserID = 1, SchoolClassID = 1 };
            ctx.Courses.AddOrUpdate(c);
            Slot s = new Slot()  {
                CourseID = 1,
                When = DateTime.Now.Date.AddHours(9)
            };
            ctx.Slots.AddOrUpdate(s);
            ctx.SaveChanges();
        }


        private void seedUsers(DbContext ctx)
        {
            string[] usernames = new string[] {
                "teacher1", "teacher2",
                "student3", "student4", "student5",
                "user6", "user7"
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

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(yalms.Models.EFContext ctx)
        {
            seedRoles(ctx);
            seedUsers(ctx);
            seedClasses(ctx);
        }
    }
}
