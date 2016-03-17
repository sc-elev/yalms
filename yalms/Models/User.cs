using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{
  
    [Table("User")]
    public class User : IdentityUser
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Lösenord")]
        public string Password  { get; set; }

        [Display(Name = "Användarnamn")]
        public string Username { get; set; }
        [Display(Name = "Roll")]
        public string Role { get; set; }





    }
}

