using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace yalms.DAL
{

    // Submitted document for a given assignment and student.
    public class Submission
    {
        [Key]
        public int SubmissionID { get; set; }

        public int AssignmentID { get; set; }

        public int UserID { get; set; }

        // FIXME: The url is more or less hardcoded 
        // as /Upload/Asssignments/AssignmentID/UserId-filename
        public string PathUrl  { get; set; }

        // Time the file was submitted. FIXME: Use File system date instead?
        public DateTime? SubmissionTime  { get; set; }

        public enum States {New, Submitted, Accepted, Rejected};
        public States State { get; set; }

        public virtual Assignment assignment { get; set;  }

    }
}

