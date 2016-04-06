using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.DAL;

namespace yalms.Models
{
    public class AssignmentNode
    {
        public string Title { set; get; }
        public IList<Assignment> Assignments { set; get; }
        public IList<AssignmentNode> Children { set; get; }

        public AssignmentNode()
        {
            Assignments = new List<Assignment>();
            Children = new List<AssignmentNode>();
        }

        public AssignmentNode(string title)
            : this()
        {
            Title = title;
        }
    }
}