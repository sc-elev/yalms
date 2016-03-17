 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace yalms.Models
{
    public class EFContext : DbContext
    {

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Couser_Student> Couser_Students { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<SchoolClassStudent> SchoolClassStudents { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<Upload> Uploads { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public EFContext(string connString)
            : base(connString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

