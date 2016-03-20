
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    public class EFContext : IdentityDbContext<DomainUser>
    {
        public EFContext() : base() { }

        public static EFContext Create() { return new EFContext(); }

        public EFContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

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



    }
}

