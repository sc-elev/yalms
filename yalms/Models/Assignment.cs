using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Assignment")]
    public class Assignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [Display(Name = "Course")]
        public int CourseID  { get; set; }

        public string Name { get; set;  }

        [Display(Name = "EndTime")]
        public DateTime? EndTime  { get; set; }

        [Display(Name = "PathUrl")]
        public string PathUrl  { get; set; }

        [Display(Name = "StartTime")]
        public DateTime? StartTime  { get; set; }

        // objects for sub key data relationship
        [NotMapped]
        public Course course { get; set; }
    }
}

