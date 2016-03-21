using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    [Table("Upload")]
    public class Upload
    {
        [Key]
        public int UploadID { get; set; }

        [Display(Name = "Assignment")]
        public int? AssignmentID  { get; set; }

        [Display(Name = "SchoolClass")]
        public int? SchoolClassID { get; set; }

        [Display(Name = "Description")]
        public string Description  { get; set; }

        [Display(Name = "Grade")]
        public int? Grade  { get; set; }

        [Display(Name = "GradeDescription")]
        public string GradeDescription  { get; set; }

        [Display(Name = "Uploaded")]
        public DateTime? Uploaded  { get; set; }

        [Display(Name = "UploadedBy")]
        public int? UploadedBy  { get; set; }


        // objects for sub key data relationship
        [NotMapped]
        public Assignment Assignment { get; set; }

   

    }
}

