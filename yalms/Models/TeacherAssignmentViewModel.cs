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

        public TeacherAssignmentViewModel() {}


        public TeacherAssignmentViewModel(int teacher_UserID, EFContext ctx)
        {
            Courses = new CourseRepository(ctx).GetAllCoursesByTeacherID_ClassAndAssignment_Full(teacher_UserID);

            CourseSelectionData = new List<SelectListItem>();
            CourseSelectionData.Add(new SelectListItem { Text = "- Ingen vald -", Value = "-1" });

            AssignmentSelectionData = new List<SelectListItem>();
            AssignmentSelectionData.Add(new SelectListItem { Text = "- Ingen vald -", Value = "-1" });
            FormSelectedAssignment = -1;

            StudentSelectionData = new List<SelectListItem>();
            StudentSelectionData.Add(new SelectListItem { Text = "- Ingen vald -", Value = "-1" });
            FormSelectedStudent = -1;

            ClassSelectionData = new List<SelectListItem>();
            ClassSelectionData.Add(new SelectListItem { Text = "- Ingen vald -", Value = "-1" });
            foreach (var course in Courses)
            {
                ClassSelectionData.Add(new SelectListItem { Text = course.SchoolClass.Name, Value = course.SchoolClass.SchoolClassID.ToString() });
            }
            FormSelectedSchoolClass = -1;
        }


    }
}