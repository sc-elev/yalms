namespace yalms.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using yalms.Models;
    using System.Collections.Generic;
    using yalms.Services;
    using yalms.DAL;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<yalms.DAL.EFContext>
    {

        private ApplicationUser student1;
        private  ApplicationUser student2;
        private  ApplicationUser student3;

        private  ApplicationUser teacher1;
        private  ApplicationUser teacher2;


        public DateTime FirstDayOfWeek(DateTime date)
        {
            var candidateDate = date;
            while (candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                candidateDate = candidateDate.AddDays(-1);
            }
            return candidateDate;
        }


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
                {
                    continue;
                }

                roleManager.Create(customRole);
            }
        }


        private void seedUsers(DbContext ctx)
        {
            // Clear the table
//ctx.Database.ExecuteSqlCommand("Delete from ")

            string[] usernames = new string[] {
                "teacher1", "teacher2",
                "student3", "student4", "student5",
                "user6", "user7"
            };

            string[] pupils = new string[] {
                "adam", "bertil", "cesar", "david", "erik", "gustav",
                "helge", "ivar", "johan", "kalle"
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
                var user = new ApplicationUser { UserName = email, Email = email };
                userManager.Create<ApplicationUser, int>(user, "1Hemlighet!");
                if (username.StartsWith("teacher"))
                    userManager.AddToRole(user.Id, "teacher");
                else if (username.StartsWith("student"))
                    userManager.AddToRole(user.Id, "student");
            }
            foreach (var username in pupils){
                var email = username + "@edu.com";
                if (userManager.FindByName(email) != null)
                    continue;
                var user = new ApplicationUser { UserName = email, Email = email };
                userManager.Create<ApplicationUser, int>(user, "1Hemlighet!");
                userManager.AddToRole(user.Id, "student");
            }
            student1 = userManager.FindByEmail("student3@edu.com");
            student2 = userManager.FindByEmail("student4@edu.com");
            student3 = userManager.FindByEmail("student5@edu.com");

            teacher1 = userManager.FindByEmail("teacher1@edu.com");
            teacher2 = userManager.FindByEmail("teacher2@edu.com");
        }


        private void seedCourses(EFContext ctx)
        {
            // Clear the table
            ctx.Courses.RemoveRange(ctx.Courses);
            ctx.SaveChanges();

            var courses = new List<Course> {
                new Course { Name = "Matematik A", SchoolClassID = 1,
                             Teacher_UserID = teacher1.Id },
                new Course { Name = "Svenska A", SchoolClassID = 1,
                             Teacher_UserID = teacher2.Id },
                new Course { Name = "SO 3", SchoolClassID = 2,
                             Teacher_UserID = teacher2.Id },
                new Course { Name = "Engelska A", SchoolClassID = 1,
                             Teacher_UserID = teacher2.Id },
                new Course { Name = "Idrott & hälsa", SchoolClassID = 1,
                             Teacher_UserID = teacher2.Id },
            };
            foreach (var course in courses) ctx.Courses.AddOrUpdate(course);
            ctx.SaveChanges();
        }


        public void seedClasses(EFContext ctx)
        {
            // Clear the table
            ctx.SchoolClasses.RemoveRange(ctx.SchoolClasses);
            ctx.SchoolClassStudents.RemoveRange(ctx.SchoolClassStudents);
            ctx.SaveChanges();

            var classes = new List<SchoolClass> {
                    new SchoolClass {Name = "7b", SchoolClassID = 1},
                    new SchoolClass {Name = "7c", SchoolClassID = 2},
                    new SchoolClass {Name = "8c", SchoolClassID = 3},
                    new SchoolClass {Name = "8f", SchoolClassID = 4},
                 };
            foreach (var class_ in classes) ctx.SchoolClasses.AddOrUpdate(class_);
            var classMembers = new List<SchoolClassStudent> {
                    new SchoolClassStudent { SchoolClassID = 1, Student_UserID = student1.Id},
                    new SchoolClassStudent { SchoolClassID = 1, Student_UserID = student2.Id},
                    new SchoolClassStudent { SchoolClassID = 2, Student_UserID = student3.Id},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 8},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 9},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 10},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 11},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 12},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 13},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 14},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 15},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 16},
                    new SchoolClassStudent { SchoolClassID = 3, Student_UserID = 17},
             };
            foreach (var member in classMembers) ctx.SchoolClassStudents.AddOrUpdate(member);
        }

        public void seedAssignments(EFContext ctx)
        {
            var assignments = new List<Assignment>  {
                new Assignment { CourseID = 1, Name = "Uppgift 1"},
                new Assignment { CourseID = 1, Name = "Uppgift 3"},
                new Assignment { CourseID = 2, Name = "Uppgift 4"},
                new Assignment { CourseID = 3, Name = "Uppgift 5"},
                new Assignment { CourseID = 3, Name = "Uppgift 6"},
                new Assignment { CourseID = 2, Name = "Uppgift 7"},
                new Assignment { CourseID = 2, Name = "Uppgift 8"},
            };
            foreach (var assignment in assignments) ctx.Assignments.AddOrUpdate(assignment);
        }


        public void seedSlots(EFContext ctx)
        {
            // Clear the table
            ctx.Rooms.RemoveRange(ctx.Rooms);
            ctx.SaveChanges();

            var rooms = new List<Room> {
                    new Room { RoomID = 1, Description = "E265", Name ="A4" },
                    new Room { RoomID = 2, Description = "F200", Name = "B3" }
            };
            foreach (var room in rooms) ctx.Rooms.AddOrUpdate(room);

            // Clear the table
            ctx.Slots.RemoveRange(ctx.Slots);
            ctx.SaveChanges();

            // Get courses for slot creation
            var createrdCourses = ctx.Courses.ToList();

            // get Rooms for Slot creatíon
            var room1 = ctx.Rooms.FirstOrDefault(o => o.Name == "A4").RoomID;
            var room2 = ctx.Rooms.FirstOrDefault(o => o.Name == "B3").RoomID;

            var monday = FirstDayOfWeek(DateTime.Today);
            var slots = new List<Slot> {
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday, SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[1].CourseID, RoomID = room2, When = monday, SlotNR = 2 },
                    new Slot {CourseID = createrdCourses[2].CourseID, RoomID = room1, When = monday, SlotNR = 3},
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday, SlotNR = 4 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room1, When = monday, SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room1, When = monday, SlotNR = 6 },
                    new Slot {CourseID = createrdCourses[4].CourseID, RoomID = room1, When = monday, SlotNR = 7 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday.AddDays(1), SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[1].CourseID, RoomID = room2, When = monday.AddDays(1), SlotNR = 2 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room2, When = monday.AddDays(1), SlotNR = 3 },
                    new Slot {CourseID = createrdCourses[4].CourseID, RoomID = room1, When = monday.AddDays(1), SlotNR = 4 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room1, When = monday.AddDays(1), SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday.AddDays(2), SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[1].CourseID, RoomID = room2, When = monday.AddDays(2), SlotNR = 2 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room2, When = monday.AddDays(2), SlotNR = 3 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room1, When = monday.AddDays(2), SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[2].CourseID, RoomID = room1, When = monday.AddDays(2), SlotNR = 6 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday.AddDays(3), SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room2, When = monday.AddDays(3), SlotNR = 4 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room2, When = monday.AddDays(3), SlotNR = 3 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room1, When = monday.AddDays(3), SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[4].CourseID, RoomID = room1, When = monday.AddDays(3), SlotNR = 7 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday.AddDays(4), SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[1].CourseID, RoomID = room2, When = monday.AddDays(4), SlotNR = 2 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room2, When = monday.AddDays(4), SlotNR = 3 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room2, When = monday.AddDays(4), SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[2].CourseID, RoomID = room1, When = monday.AddDays(4), SlotNR = 6 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room1, When = monday.AddDays(7), SlotNR = 1 },
                    new Slot {CourseID = createrdCourses[1].CourseID, RoomID = room2, When = monday.AddDays(7), SlotNR = 2 },
                    new Slot {CourseID = createrdCourses[0].CourseID, RoomID = room2, When = monday.AddDays(7), SlotNR = 3 },
                    new Slot {CourseID = createrdCourses[3].CourseID, RoomID = room2, When = monday.AddDays(7), SlotNR = 5 },
                    new Slot {CourseID = createrdCourses[2].CourseID, RoomID = room1, When = monday.AddDays(7), SlotNR = 6 },
                    new Slot {CourseID = createrdCourses[4].CourseID, RoomID = room1, When = monday, SlotNR = 7 },
            };
            foreach (var slot in slots) ctx.Slots.AddOrUpdate(slot);
            ctx.SaveChanges();

        }


        public void seedSubmissions(EFContext ctx)
        {
            var submissions = new List<Submission> {
                new Submission { AssignmentID = 1, UserID = student1.Id, State = Submission.States.Accepted },
                new Submission { AssignmentID = 2, UserID = student1.Id, State = Submission.States.Rejected },
                new Submission { AssignmentID = 3, UserID = student1.Id, State = Submission.States.New },
                new Submission { AssignmentID = 4, UserID = student1.Id, State = Submission.States.Accepted },
                new Submission { AssignmentID = 5, UserID = student1.Id, State = Submission.States.Rejected },
                new Submission { AssignmentID = 6, UserID = student1.Id, State = Submission.States.Accepted },
                new Submission { AssignmentID = 1, UserID = student2.Id, State = Submission.States.Accepted },
                new Submission { AssignmentID = 2, UserID = student2.Id, State = Submission.States.Accepted },
            };
            foreach (var submission in submissions) ctx.Submissions.AddOrUpdate(submission);
            ctx.SaveChanges();
        }


        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(yalms.DAL.EFContext ctx)
        {
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            // -- Enables otherwise problematic debugging of this method.
            seedRoles(ctx);
            seedUsers(ctx);
            seedCourses(ctx);
            seedClasses(ctx);
            seedSlots(ctx);
            seedAssignments(ctx);
            seedSubmissions(ctx);
        }
    }
}
