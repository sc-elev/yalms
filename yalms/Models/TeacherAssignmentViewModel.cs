using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using yalms.DAL;

namespace yalms.Models
{
    public class TeacherAssignmentViewModel
    {
        // Populate courses with Classes that is populated with students and assignments
        //public List<Course> Courses { get; set; }
        //public List<ListItem> ViewSelect { get; set; }

        public IEnumerable<Course> Courses { get; set; }
        public List<SelectListItem> CourseSelectionData { get; set; }
        public List<SelectListItem> ViewSelectionData { get; set; }

        public List<SelectListItem> ClassSelectionData { get; set; }
        public int FormSelectedSchoolClass { get; set; }
 
        public List<SelectListItem> AssignmentSelectionData { get; set; }
        public int FormSelectedAssignment { get; set; }

        public List<SelectListItem> StudentSelectionData { get; set; }
        public int FormSelectedStudent { get; set; }

        public int FormSelectedView { get; set; }

        public IList<AssignmentNode> StudentAssignments { set; get; }
        

        public TeacherAssignmentViewModel() {}


        public TeacherAssignmentViewModel(int teacher_UserID, EFContext ctx)
        {
            Courses = new CourseRepository(ctx).GetAllCoursesByTeacherID_ClassAndAssignment_Full(teacher_UserID);

            CourseSelectionData = new List<SelectListItem>();
            CourseSelectionData.Add(new SelectListItem { Text = "- Välj klass först -", Value = "-1" });

            AssignmentSelectionData = new List<SelectListItem>();
            AssignmentSelectionData.Add(new SelectListItem { Text = "- Välj klass först -", Value = "-1" });
            FormSelectedAssignment = -1;

            StudentSelectionData = new List<SelectListItem>();
            StudentSelectionData.Add(new SelectListItem { Text = "- Välj klass först -", Value = "-1" });
            FormSelectedStudent = -1;

            ClassSelectionData = new List<SelectListItem>();
            ClassSelectionData.Add(new SelectListItem { Text = "- Ingen vald -", Value = "-1" });
            foreach (var course in Courses)
            {
                ClassSelectionData.Add(new SelectListItem { Text = course.SchoolClass.Name, Value = course.SchoolClass.SchoolClassID.ToString() });
            }
            FormSelectedSchoolClass = -1;

            

            StudentAssignments = new List<AssignmentNode>();
            foreach (var course in Courses)
            {
                var classCourse = new AssignmentNode();
                classCourse.Title = course.Name + " - " + course.SchoolClass.Name;

                var assignments = new AssignmentRepository().GetAllAssignmentsByCourseID(course.CourseID);
                foreach (var assignment in assignments)
                {
                    var assignmentNode = new AssignmentNode();
                    assignmentNode.Title = assignment.Name;

                    //Add Categories
                    var approvedNode = new AssignmentNode();
                    approvedNode.Title = "Godkända";
                    assignmentNode.Children.Add(approvedNode);

                    var rejectedNode = new AssignmentNode();
                    rejectedNode.Title = "Ej godkända";
                    assignmentNode.Children.Add(rejectedNode);

                    // Add students.
                    foreach (var student in course.SchoolClass.Students)
                    {
                        var studentNode = new AssignmentNode();
                        studentNode.Title = student.UserName;

                        var Alle = 1;

                        // var studAssign = UploadPaths.
                        //foreach (var assignment in course.Assignments)

                        //studentNode.Children.Add(approvedNode);

                        //var rejectedNode = new AssignmentNode();
                        //rejectedNode.Title = "Ej godkända";
                        //studentNode.Children.Add(rejectedNode);

                        // Add student Node
                        assignmentNode.Children.Add(studentNode);
                    }

                    classCourse.Children.Add(assignmentNode);
                }
                //category.Assignments = context.GetAssignments()
                //    .Where(a => a.CourseID == course.CourseID)
                //    .ToList();
                //if (category.Assignments.Count > 0)

                // Add ClassCourse Node
                StudentAssignments.Add(classCourse);
            }
        }


    }
}