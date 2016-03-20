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

namespace yalms.Models
{
  
    [Table("User")]
    public class DomainUser : 
        IdentityUser<int, CustomUserLogin, CustomUserRole,  CustomUserClaim> 
    {
        public DomainUser(string userName) : base() 
        {
            CreatedAt = DateTime.Now;
            UserName = userName;
        }

        public DomainUser() : base() 
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { set; get; }

        public async Task<ClaimsIdentity> 
            GenerateUserIdentityAsync(UserManager<DomainUser , int> manager)
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

