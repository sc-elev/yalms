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
        public IList<Slot> slots { get; set;  }
        public IList<Slot> SlotTimings { get; set;  }

        public StudentMainViewModel(YalmContext context,
                                    int studentID, 
                                    DateTime when)
        {
            slots = (from slot in context.GetSlots()
                     join cour in context.GetCourses()
                         on slot.CourseID equals cour.CourseID
                     join cost in context.GetCourse_Students()
                         on cour.CourseID equals cost.CourseID
                     where cost.Student_UserID == studentID
                         && slot.When.DayOfYear == when.DayOfYear
                     select slot).ToList();
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");
            WeekNr = cultureInfo.Calendar.GetWeekOfYear(
                when, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            WeekDay = cultureInfo.DateTimeFormat.GetDayName(when.DayOfWeek);
        }
        
        public StudentMainViewModel() { }

    }
}