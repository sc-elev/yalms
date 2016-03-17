using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Couser_Student")]
    public class Couser_Student
    {
        [Key]
        public int Couser_StudentID { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Course")]
        public int CourseID  { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Student_User")]
        public int Student_UserID  { get; set; }


        // objects for sub key data relationship
        [NotMapped]
        public Course CourseID_Course { get; set; }
        [NotMapped]
        public User Student_UserID_Student_User { get; set; }



    }
}

