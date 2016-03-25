using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Models
{

    public class StudentMainViewModelFactory
    {
        private Controller controller;
        private EFContext context;
        private IUserProvider userProvider;
        
        public StudentMainViewModelFactory(Controller controller, 
                                           EFContext context,  
                                           IUserProvider user)
        {
            this.controller = controller;
            this.context = context;
            this.userProvider = user;
        }


        public StudentMainViewModel Create( IDateProvider date, int DaysToAdd = 0)
        {
            StudentMainViewModel model =
                 controller.TempData["studentViewModel"] as StudentMainViewModel;

            var nextDate = date != null ? date.Today() : DateTime.Now.Date;
            if (model != null && date == null)
            {
                nextDate = model.Today;   
            }
            nextDate = nextDate.AddDays(DaysToAdd);
            var nextDay = new DummyDateProvider(nextDate);
            model =
                new StudentMainViewModel(context, userProvider.Who(), nextDay);
            controller.TempData["studentViewModel"] = model;
            return model;
        }
    }


    public class StudentMainViewModel
    {
        public string WeekDay { set; get; }
        public int WeekNr  { set; get; }
        public string Date { set; get; }
        public DateTime Today { set; get; }
        public IList<Slot> slots { get; set; }
        public IList<TimingInfo> SlotTimings { get; set;  }
        public string SchoolClass { get; set; }

        public static StudentMainViewModel Create(
            Controller controller, 
            EFContext context,  
            UserProvider user, 
            DateProvider date,
            int DaysToAdd = 0) 
        {
            StudentMainViewModel model =
                 controller.TempData["tudentViewModel"] as StudentMainViewModel;
            var nextDay =
                new DummyDateProvider(model.Today.AddDays(DaysToAdd));
            var dateProvider = new DummyDateProvider(nextDay.Today());
            model =
                new StudentMainViewModel(context, user.Who(), dateProvider);
            controller.TempData["StudentViewModel"] = model;
            return model;
        }

        public StudentMainViewModel(YalmContext context,
                                    string studentName, 
                                    IDateProvider dateProvider)
        {

            ApplicationUser user = context.GetUsers()
                .Where(u => u.UserName == studentName)
                .SingleOrDefault();
            SchoolClassStudent scs = context.GetSchoolClassStudents()
                .Where(s => s.Student_UserID == user.Id)
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
