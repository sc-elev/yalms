using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using yalms.Models;


namespace yalms.DAL
{
  
    [Table("User")]
    public class ApplicationUser : 
        IdentityUser<int, CustomUserLogin, CustomUserRole,  CustomUserClaim> 
    {
        public ApplicationUser(string userName) : base() 
        {
            CreatedAt = DateTime.Now;
            UserName = userName;
        }

        public ApplicationUser(string userName, int Id)
            : base()
        {
            CreatedAt = DateTime.Now;
            UserName = userName;
            this.Id = Id;
        }
        public ApplicationUser() : base() 
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { set; get; }

        public async Task<ClaimsIdentity> 
            GenerateUserIdentityAsync(UserManager<ApplicationUser , int> manager)
        {
            // Note the authenticationType must match the one defined in 
            // CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}

