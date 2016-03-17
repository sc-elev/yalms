
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace yalms.Models
{
    //public class DXContext : IdentityDbContext<User, Role,
    //int, UserLogin, UserRole, UserClaim>//: DbContext
    //{
    //    public DXContext()
    //        : base("name=DXContext")
    //    {
    //        Database.SetInitializer<DXContext>(null);// Remove default initializer
    //        Configuration.ProxyCreationEnabled = false;
    //        Configuration.LazyLoadingEnabled = false;
    //    }

    //    public static DXContext Create()
    //    {
    //        return new DXContext();
    //    }

    //    //Identity and Authorization
    //    public DbSet<UserLogin> UserLogins { get; set; }
    //    public DbSet<UserClaim> UserClaims { get; set; }
    //    public DbSet<UserRole> UserRoles { get; set; }

    //    // ... your custom DbSets
    //    public DbSet<RoleOperation> RoleOperations { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    //        modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

    //        // Configure Asp Net Identity Tables
    //        modelBuilder.Entity<User>().ToTable("User");
    //        modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
    //        modelBuilder.Entity<User>().Property(u => u.Stamp).HasMaxLength(500);
    //        modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

    //        modelBuilder.Entity<Role>().ToTable("Role");
    //        modelBuilder.Entity<UserRole>().ToTable("UserRole");
    //        modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
    //        modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
    //        modelBuilder.Entity<UserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
    //        modelBuilder.Entity<UserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);
    //    }
    //}



    public class EFContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public EFContext()
            : base("name=EFContext")
            {
                Database.SetInitializer<EFContext>(null);// Remove default initializer
                Configuration.ProxyCreationEnabled = false;
                Configuration.LazyLoadingEnabled = false;
            }

        public static EFContext Create()
            {
                return new EFContext();
            }

            //Identity and Authorization
            public DbSet<UserLogin> UserLogins { get; set; }
            public DbSet<UserClaim> UserClaims { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

                // Configure Asp Net Identity Tables
                modelBuilder.Entity<User>().ToTable("User");
                modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
                modelBuilder.Entity<User>().Property(u => u.Stamp).HasMaxLength(500);
                modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

                modelBuilder.Entity<Role>().ToTable("Role");
                modelBuilder.Entity<UserRole>().ToTable("UserRole");
                modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
                modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
                modelBuilder.Entity<UserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
                modelBuilder.Entity<UserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);
            }

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

        public DbSet<Role> UserTypes { get; set; }

        //public EFContext()
        //    : base()
        //{
        //}

        //public EFContext(string connString)
        //    : base(connString)
        //{
        //}
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}

