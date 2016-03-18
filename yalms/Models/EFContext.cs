
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

        public EFContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



    //        //Identity and Authorization
    //    public DbSet<IdentityUserLogin> UserLogins { get; set; }
    //    public DbSet<IdentityUserClaim> UserClaims { get; set; }
    //        public DbSet<IdentityUserRole> UserRoles { get; set; }

    //        protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //        {
    //            base.OnModelCreating(modelBuilder);

    //            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    //            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

    //            // Configure Asp Net Identity Tables
    //            modelBuilder.Entity<User>().ToTable("User");
    //            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
    //            modelBuilder.Entity<User>().Property(u => u.Stamp).HasMaxLength(500);
    //            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

    //            modelBuilder.Entity<Role>().ToTable("Role");
    //            modelBuilder.Entity<UserRole>().ToTable("UserRole");
    //            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
    //            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
    //            modelBuilder.Entity<UserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
    //            modelBuilder.Entity<UserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);
    //        }

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

