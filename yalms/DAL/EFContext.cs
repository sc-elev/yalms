
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

        IList<Assignment> GetAssignments(); 

        IList<Course> GetCourses();

        IList<Room> GetRooms();

        IList<SchoolClass> GetSchoolClasses();

        IList<SchoolClassStudent> GetSchoolClassStudents();

        IList<Slot> GetSlots();

        IList<Upload> GetUploads();

        IList<ApplicationUser> GetUsers();

        IList<Submission> GetSubmissions();

        void Dispose();
    }


    public class EFContext : 
        IdentityDbContext<ApplicationUser, CustomRole,
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
    
        public virtual DbSet<Assignment> Assignments { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

        public virtual DbSet<SchoolClassStudent> SchoolClassStudents { get; set; }

        public virtual DbSet<Slot> Slots { get; set; }

        public virtual DbSet<Upload> Uploads { get; set; }

        public virtual DbSet<Submission> Submissions { get; set; }

        public virtual IList<Assignment> GetAssignments()
        {
            return Assignments.ToList();
        }

        public virtual IList<Course> GetCourses()
        {
            return Courses.ToList();
        }

        public virtual IList<Room> GetRooms()
        {
            return Rooms.ToList();
        }

        public virtual IList<SchoolClass> GetSchoolClasses()
        {
            return SchoolClasses.ToList();
        }

        public virtual IList<SchoolClassStudent> GetSchoolClassStudents()
        {
            return SchoolClassStudents.ToList();
        }

        public virtual IList<Slot> GetSlots()
        {
            return Slots.ToList();
        }

        public virtual IList<Upload> GetUploads()
        {
            return Uploads.ToList();
        }

        public virtual IList<ApplicationUser> GetUsers()
        {
            return Users.ToList();
        }
        public virtual IList<Submission> GetSubmissions()
        {
            return Submissions.ToList();
        }

    }
}

