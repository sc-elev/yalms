using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Role")]
    public class Role //: IdentityRole
    {
        [Key]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Rolename")]
        public string Rolename  { get; set; }
        



    }
}

