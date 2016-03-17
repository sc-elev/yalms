using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{
   /// <summary>
   /// ---------------------------------------------------------------------------------------------------
   /// File Autogenerated at 2016-03-16 23:23:37
   /// Object derived from the  Course table in the LMSProject database.
   /// Solution version: 1.0.0.1
   /// [ ] Check box with 'x' to prevent entire file from being overwritten. 
   /// Do not remove or change the DMZ region markers. Code inside the DMZ will not be overwritten by auto generation.
   /// ---------------------------------------------------------------------------------------------------
   /// </summary>
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
        public SchoolClass SchoolClassID_SchoolClass { get; set; }
        [NotMapped]
        public User Teacher_UserID_Teacher_User { get; set; }

        // System Objects.
        [NotMapped] 
        public User CreatedBy_User { get; set; }

        [NotMapped] 
        public User ModifiedBy_User { get; set; }

        [NotMapped] 
        public User RemovedBy_User { get; set; }

        #region DMZ - Custome Code placed in the DMZ will be protected from system overwrites.


        #endregion DMZ End

    }
}

