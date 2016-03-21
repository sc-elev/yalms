using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace yalms.Models
{
    public class TeascherAssignmentViewModel
    {
        // Populate courses with Classes that is populated with students and assignments
        public List<Course> Courses { get; set; }
        public List<ListItem> ViewSelect { get; set; }

    }
}