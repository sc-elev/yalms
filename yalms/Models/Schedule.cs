using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Display(Name = "AssignmentFolder")]
        public string AssignmentFolder  { get; set; }

        [Display(Name = "SchoolClass")]
        public int? SchoolClassID  { get; set; }


        // objects for sub key data relationship


    }
}

