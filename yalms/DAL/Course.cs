using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.DAL
{

    [Table("Course")]
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Display(Name = "Description")]
        public string Description  { get; set; }

        [Display(Name = "Name")]
        public string Name  { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "SchoolClass")]
        public int SchoolClassID  { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Teacher_User")]
        public int Teacher_UserID  { get; set; }


        // objects for sub key data relationship
        [NotMapped]
        public SchoolClass SchoolClass { get; set; }
        [NotMapped]
        public ApplicationUser Teacher_User { get; set; }
        [NotMapped]
        public List<Assignment> Assignments { get; set; }
        [NotMapped]
        public List<Slot> Slots { get; set; }

    }
}

