using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.DAL;

namespace yalms.Models
{
    public class SubmissionsNode
    {
        public string Title { set; get; }
        public IList<Submission> Submissions { set; get; }
        public IList<SubmissionsNode> Children { set; get; }

        public SubmissionsNode()
        {
            Submissions = new List<Submission>();
            Children = new List<SubmissionsNode>();
        }

        public SubmissionsNode(string title)
            : this()
        {
            Title = title;
        }
    }
}