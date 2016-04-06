using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.DAL
{

    [Table("SchoolClass")]
    public class SchoolClass
    {
        [Key]
        public int SchoolClassID { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Name")]
        public string Name  { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "SharedClassFolderUrl")]
        public int SharedClassFolderUrl  { get; set; }

        [Required(ErrorMessage = "Field can not be empty.")]
        [Display(Name = "Year")]
        public int Year  { get; set; }

        [NotMapped]
        public List<ApplicationUser> Students { get; set; }
    }
}

