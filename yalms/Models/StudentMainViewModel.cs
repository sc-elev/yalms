using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace yalms.Models
{

    public class StudentMainViewModel
    {
        public string WeekDay { set; get; }
        public int WeekNr  { set; get; }
        public string Date { set; get; }
        public DateTime Today { set; get; }
        public IList<Slot> slots { get; set; }
        public IList<Slot> SlotTimings { get; set;  }
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
            Date = dateProvider.Today().ToString("yyyy-MM-dd");
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");
            Today = dateProvider.Today();
            WeekNr = cultureInfo.Calendar.GetWeekOfYear(
                Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            WeekDay = cultureInfo.DateTimeFormat.GetDayName(Today.DayOfWeek);
       }
        
        public StudentMainViewModel() { }

    }
}
