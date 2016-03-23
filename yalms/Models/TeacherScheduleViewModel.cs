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
        public List<SelectListItem> CourseSelectionData { get; set; }
        public int SelectedCourse { get; set; }
        public int FormSelectedCourse { get; set; }

        public List<SelectListItem> RoomSelectionData { get; set; }
        public int FormSelectedRoom { get; set; }

        public List<TimingInfo> SlotTimings { get; set; }

        public Slot SelectedSlot { get; set; }

        public DateTime FirstDayOfWeek { get; set; }

        public TeacherScheduleViewModel() {}

        public TeacherScheduleViewModel(DateTime date, int teacher_UserID)
        {
            SlotTimings = new List<TimingInfo>(SlotTimingInfo.Timings);

            FirstDayOfWeek = CommonFunctions.CustomConversion.GetFirstDayOfWeekFromDate(date);

            // populate rooms
            var Rooms = new RoomRepository().GetAllRooms();
            RoomSelectionData = new List<SelectListItem>();
            foreach (var room in Rooms) {
                RoomSelectionData.Add(new SelectListItem { Text = room.Name, Value = room.RoomID.ToString() });
            }

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