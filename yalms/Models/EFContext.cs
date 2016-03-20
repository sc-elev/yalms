
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace yalms.Models
{
    public interface YalmContext 
    {
        void OnModelCreating(DbModelBuilder modelBuilder);

        IList<Assignment> GetAssignments(); 

        IList<Course> GetCourses();

        IList<Course_Student> GetCourse_Students();

        IList<Room> GetRooms();

        IList<SchoolClass> GetSchoolClasses();

        IList<SchoolClassStudent> GetSchoolClassStudents();

        IList<Slot> GetSlots();

        IList<Upload> GetUploads();
    }


    public class EFContext : 
        IdentityDbContext<DomainUser, CustomRole,
                          int, CustomUserLogin, CustomUserRole, 
                          CustomUserClaim>, 
        YalmContext
    {
        public EFContext() : base("DefaultConnection") { }

        public static EFContext Create() { return new EFContext(); }

        public EFContext(string nameOrConnectionString) : 
            base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Course_Student> Course_Students { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<SchoolClassStudent> SchoolClassStudents { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<Upload> Uploads { get; set; }


        void YalmContext.OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }

        IList<Assignment> YalmContext.GetAssignments()
        {
            return Assignments.ToList();
        }

        IList<Course> YalmContext.GetCourses()
        {
            return Courses.ToList();
        }

        IList<Course_Student> YalmContext.GetCourse_Students()
        {
            return Course_Students.ToList();
        }

        IList<Room> YalmContext.GetRooms()
        {
            return Rooms.ToList();
        }

        IList<SchoolClass> YalmContext.GetSchoolClasses()
        {
            return SchoolClasses.ToList();
        }

        IList<SchoolClassStudent> YalmContext.GetSchoolClassStudents()
        {
            return SchoolClassStudents.ToList();
        }

        IList<Slot> YalmContext.GetSlots()
        {
            return Slots.ToList();
        }

        IList<Upload> YalmContext.GetUploads()
        {
            return Uploads.ToList();
        }
    }
}

