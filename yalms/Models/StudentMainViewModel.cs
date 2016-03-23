using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using yalms.DAL;

namespace yalms.Models
{

    public class StudentMainViewModel
    {
        public string WeekDay { set; get; }
        public int WeekNr  { set; get; }
        public string Date { set; get; }
        public DateTime Today { set; get; }
        public IList<Slot> slots { get; set; }
        public IList<TimingInfo> SlotTimings { get; set;  }
        public string SchoolClass { get; set; }

        public StudentMainViewModel(YalmContext context,
                                    string studentName, 
                                    IDateProvider dateProvider)
        {

            ApplicationUser user = context.GetUsers()
                .Where(u => u.UserName == studentName)
                .SingleOrDefault();
            SchoolClassStudent scs = context.GetSchoolClassStudents()
                .Where(s => s.SchoolClassStudentID == user.Id)
                .SingleOrDefault();
            SchoolClass sc = context.GetSchoolClasses()
                .Where(c => c.SchoolClassID == scs.SchoolClassID)
                .SingleOrDefault();

            SchoolClass = sc.Name;
            slots = context.GetSlots()
                            .Where(s => s.When.Date == dateProvider.Today())
                            .ToList();
            foreach (var slot in slots)
            {
                slot.Course = context.GetCourses()
                    .Where(c => c.CourseID == slot.CourseID)
                    .SingleOrDefault();
                slot.Room = context.GetRooms()
                    .Where(r => r.RoomID == slot.RoomID)
                    .SingleOrDefault();

                slot.Course.SchoolClass = context.GetSchoolClasses()
                    .Where(s => s.SchoolClassID == slot.Course.SchoolClassID)
                    .SingleOrDefault();
            }
            slots = slots
                        .Where(s => s.Course.SchoolClassID == sc.SchoolClassID)
                        .OrderBy(w => w.SlotNR)
                        .ToList();
            Today = dateProvider.Today();
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");

            var month = CultureInfo
                            .CurrentCulture
                            .DateTimeFormat
                            .GetAbbreviatedMonthName(Today.Month);
            Date = Today.Day + " " + month;
            WeekNr = cultureInfo.Calendar.GetWeekOfYear(
                Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            WeekDay = cultureInfo.DateTimeFormat.GetDayName(Today.DayOfWeek);
            SlotTimings = new List<TimingInfo>(SlotTimingInfo.Timings);
       }

        
        public StudentMainViewModel() { }

    }
}
