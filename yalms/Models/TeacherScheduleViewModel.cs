using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Models
{
    public class TeacherScheduleViewModel
    {
        public List<Course> Courses { get; set; }
        public int SelectedCourse { get; set; }
        public SlotTimingInfo Timings { get; set; }
        public List<SelectListItem> CourseSelectionData { get; set; }

        public TeacherScheduleViewModel() {}

        public TeacherScheduleViewModel(DateTime date, int teacher_UserID)
        {
            Timings = new SlotTimingInfo();
          
            // populate full courses data.
            Courses = new CourseRepository().GetAllCoursesByTeacherIDAndWeek_Full(teacher_UserID, date).ToList();

            // Check if courses found
            if (Courses.Count != 0) {
                SelectedCourse = Courses.FirstOrDefault().CourseID;

                CourseSelectionData = new List<SelectListItem>();
                foreach (var course in Courses)
                {
                    CourseSelectionData.Add(new SelectListItem { Text = course.Name, Value = course.CourseID.ToString() });
                }
            }
            else
            {
                SelectedCourse = -1;
            }
        }
    }
}