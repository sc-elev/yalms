using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Slot")]
    public class Slot
    {
        [Key]
        public int SlotID { get; set; }

        [Display(Name = "Course")]
        public int? CourseID  { get; set; }

        [Display(Name = "Room")]
        public int? RoomID  { get; set; }

        [Display(Name = "SlotNR")]
        public int? SlotNR  { get; set; }

        [Display(Name = "WeekDay")]
        public int? WeekDay  { get; set; }

        [Display(Name = "WeekNR")]
        public int? WeekNR  { get; set; }


        // objects for sub key data relationship
        [NotMapped]
        public Course CourseID_Course { get; set; }
        [NotMapped]
        public Room RoomID_Room { get; set; }


    }
}

