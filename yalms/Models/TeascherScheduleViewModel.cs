using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.DAL;

namespace yalms.Models
{
    public class TeascherScheduleViewModel
    {
        List<Slot> Slots { get; set; }

        public TeascherScheduleViewModel() {}

        public TeascherScheduleViewModel(int courseID, DateTime date)
        {


            Slots = new SlotRepository().GetTeachersWeeklySheduleByCourseIDAndDate_Full(courseID, date);
        }
    }
}