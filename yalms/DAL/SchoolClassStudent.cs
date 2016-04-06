using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.DAL
{

    [Table("SchoolClassStudent")]
    public class SchoolClassStudent
    {
        [Key]
        public int SchoolClassStudentID { get; set; }

        [Display(Name = "SchoolClass")]
        public int? SchoolClassID  { get; set; }

        [Display(Name = "Student_User")]
        public int? Student_UserID  { get; set; }


        // objects for sub key data relationship
        [NotMapped]
        public ApplicationUser Student { get; set; }


    }
}

