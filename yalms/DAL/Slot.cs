using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.DAL
{

    [Table("Slot")]
    public class Slot
    {
        [Key]
        public int SlotID { get; set; }

        [Display(Name = "Course")]
        public int CourseID  { get; set; }

        [Display(Name = "Room")]
        public int RoomID  { get; set; }

        [Display(Name = "SlotNR")]
        public int SlotNR  { get; set; }

        // Start time of this slot
        public DateTime When { get; set;  }


        // objects for sub key data relationship
        [NotMapped]
        public Course Course { get; set; }
        [NotMapped]
        public Room Room { get; set; }


    }
}

