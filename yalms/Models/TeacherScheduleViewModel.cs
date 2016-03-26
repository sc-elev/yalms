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

        int foo = SlotTimingInfo.Timings.Count;

        // All slots this week, conceptually slots[slotNr, weekday]
        public Slot[,] ThisWeekSlots  { get; set;}

        // The callback url related to each slot in ThisWeekSlots
        public string[,] ThisWeekUrls { get; set; }

        public TeacherScheduleViewModel() {}


        public TeacherScheduleViewModel(DateTime date, int teacher_UserID, EFContext ctx)
        {
            SlotTimings = new List<TimingInfo>(SlotTimingInfo.Timings);

            FirstDayOfWeek = CommonFunctions.CustomConversion.GetFirstDayOfWeekFromDate(date);

            // populate rooms
            var Rooms = ctx.GetRooms();
            RoomSelectionData = new List<SelectListItem>();
            foreach (var room in Rooms) {
                RoomSelectionData.Add(new SelectListItem { Text = room.Name, Value = room.RoomID.ToString() });
            }

            // populate full courses data.
            Courses = new CourseRepository(ctx).GetAllCoursesByTeacherIDAndWeek_Full(teacher_UserID, date).ToList();

            // Check if courses found
            if (Courses.Count != 0) {
                SelectedCourse = Courses.FirstOrDefault().CourseID;

                CourseSelectionData = new List<SelectListItem>();
                foreach (var course in Courses)
                {
                    CourseSelectionData.Add(new SelectListItem { Text = course.Name + " "+ course.SchoolClass.Name, Value = course.CourseID.ToString() });
                }
            }
            else
            {
                SelectedCourse = -1;
            }
            ThisWeekSlots = new Slot[SlotTimingInfo.Timings.Count, 5];
            ThisWeekUrls = new string[SlotTimingInfo.Timings.Count, 5];

            for (var i = 0; i < 5; i++)
            {
                var course = Courses.FirstOrDefault(o => o.CourseID == SelectedCourse);
                var dailyDate = FirstDayOfWeek.AddDays(i);
                var dailySlots = course.Slots.Where(o => o.When.Date == dailyDate.Date);

                for (var row = 1; row < SlotTimings.Count; row++)
                {
                    var slot = dailySlots.FirstOrDefault(o => o.SlotNR == row);
                    if (slot != null)
                    {
                        ThisWeekSlots[row, i] = slot;
                    }
                    else
                    {
                        ThisWeekSlots[row, i] = null;
                    }
                }
            }
        }


        private Slot CopySlot(Slot slot)
        {
            return new Slot {
                SlotID = slot.SlotID,
                SlotNR = slot.SlotNR,
                When = slot.When
            };
        }


        public void BuildSlotUrls(UrlHelper urlHelper)
        {
            for (var day = 0; day < 5; day += 1)
            {
                var weekDay = FirstDayOfWeek.AddDays(day);
                for (var row = 0; row < SlotTimingInfo.Timings.Count; row += 1)
                {
                    if (ThisWeekSlots[row, day] != null)
                    {
                        ThisWeekUrls[row, day] = urlHelper.Action(
                            "SlotClick", "Teacher",
                            CopySlot(ThisWeekSlots[row, day]));
                    }
                    else
                    {
                        ThisWeekUrls[row, day] = urlHelper.Action(
                            "SlotClick", "Teacher",
                            new Slot { SlotID = -1, SlotNR = row, When = weekDay });
                    }
                }
            }
        }

    }
}
