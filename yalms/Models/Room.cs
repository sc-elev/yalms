using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Room")]
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Display(Name = "Description")]
        public string Description  { get; set; }



    }
}

