using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

// must be present for 
using Microsoft.AspNet.Identity;
using yalms.Services;
using yalms.DAL;
using yalms.CommonFunctions;

namespace yalms.Controllers
{
      
    [Authorize(Roles = "teacher")]
    public class TeacherController : Controller
    {
        static TeacherScheduleViewModel teacherScheduleViewModel;

        protected IDateProvider dateProvider;
        protected IUserProvider userProvider;
        protected EFContext context;

        // Declare all Used repositories here
        private ISchoolClassStudentRepository schoolClassStudentRepository;
        private ICourseRepository courseRepository;
        private IAssignmentRepository assignmentRepository;
        private ISlotRepository slotRepository;

        // GET: Teacher
        public ViewResult Administration()
        {
            // viewmodel: TeacherAdministrationViewModel
            return View();
        }

        public ViewResult Assignment()
        {
            // viewmodel: TeacherAssignmentViewModel
            var teacher_UserID = -1;
            try
            {
                teacher_UserID = this.userProvider.UserID();
            }
            catch { }

            var modelFactory = new TeacherAssignmentViewModelFactory(this, context, this.userProvider);
            var model = modelFactory.Create();

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadStudentsByClass(string id)
        {
            this.schoolClassStudentRepository = new SchoolClassStudentRepository(context);

            var studentList = schoolClassStudentRepository.GetAllSchoolClassStudentsBySchoolClassID_Full(Convert.ToInt32(id));

            var studentData = studentList.Select(m => new SelectListItem()
            {
                Text = m.Student.UserName,
                Value = m.Student_UserID.ToString()
            });
            return Json(studentData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadAssignmentsByClass(string id)
        {
            this.courseRepository = new CourseRepository(context);
            this.assignmentRepository = new AssignmentRepository(context);

            var courseID = courseRepository.GetCourseByClassID(Convert.ToInt32(id)).CourseID;

            // Get Assignments
            var assignmentList = assignmentRepository.GetAllAssignmentsByCourseID(courseID);
            var studentData = assignmentList.Select(m => new SelectListItem()
            {
                Text = m.Name,
                Value = m.AssignmentID.ToString()
            });
            return Json(studentData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadAllStudentAssgnments(TeacherAssignmentViewModel viewModel)
        {
            //var alle = 1;
            //RedirectToAction()

            return null;
        }

        public ViewResult Document()
        {
            // viewmodel: TeacherDocumentViewModel
            return View();
        }

        public ViewResult Schedule()
        {
            // viewmodel: TeacherScheduleViewModel
            // DateTime selectedDate = Session["selectedDate"] ? ;
            var selectedDate = dateProvider.Today();
            TeacherScheduleViewModel model; //= new TeacherScheduleViewModel();
            //if (Session == null)
            //{
            if (teacherScheduleViewModel == null)
            {
                var modelFactory = new TeacherScheduleViewModelFactory(this, context, this.userProvider);
                model = modelFactory.Create(selectedDate) as TeacherScheduleViewModel;
                teacherScheduleViewModel = model;
            }
            else
            {
                model = teacherScheduleViewModel;
            }

 

            UrlHelper urlHelper;
            if (this.Request != null)
            {
                urlHelper = new UrlHelper(this.Request.RequestContext);
            }
            else
            {
                urlHelper = new UrlHelper();
            }

            //if (this.Request != null) { model.BuildSlotUrls(urlHelper); }

            for (var day = 0; day < 5; day += 1)
            {
                var weekDay = model.FirstDayOfWeek.AddDays(day);
                for (var row = 0; row < SlotTimingInfo.Timings.Count; row += 1)
                {
                    if (model.ThisWeekSlots[row, day] != null)
                    {
                        model.ThisWeekUrls[row, day] = urlHelper.Action(
                            "SlotClick", "Teacher",
                            model.CopySlot(model.ThisWeekSlots[row, day]));
                    }
                    else
                    {
                        try
                        {
                            model.ThisWeekUrls[row, day] = urlHelper.Action(
                                "SlotClick", "Teacher",
                                new Slot { SlotID = -1, SlotNR = row, When = weekDay });
                        }
                        catch (System.ArgumentNullException)
                        {
                            // Unit testing: No Request available.
                            model.ThisWeekUrls[row, day] = "http://not-defined";
                        }
                    }
                }
            }

            if (model.SelectedSlot != null)
            {
                if (model.SelectedSlot.SlotID != -1)
                {
                    try
                    {
                        model.FormSelectedCourse = (int)model.SelectedSlot.CourseID;
                    }
                    catch { }

                    try
                    {
                        model.FormSelectedRoom = (int)model.SelectedSlot.RoomID;
                    }
                    catch { }
                }


                ViewBag.SelectedSlotInformation = model.SelectedSlot.When.ToShortDateString()
                    + " (" + model.SlotTimings[model.SelectedSlot.SlotNR].start.ToLongTimeString().Substring(0, 5)
                    + " - " + model.SlotTimings[model.SelectedSlot.SlotNR].end.ToLongTimeString().Substring(0, 5) + ")";

            } else {
                ViewBag.SelectedSlotInformation ="- Ingen kalender ruta vald -";
            }

            return View(model);
        }



        [HttpPost]
        [MultipleButton(Name = "action", Argument ="Save")]
        public ActionResult Save(TeacherScheduleViewModel pageviewmodel)
        {
            var model = teacherScheduleViewModel;

            if (model.SelectedSlot != null && pageviewmodel.FormSelectedCourse != -1 && pageviewmodel.FormSelectedRoom != -1)
            {
                this.slotRepository = new SlotRepository(context);

                // update from form
                model.SelectedSlot.CourseID = pageviewmodel.FormSelectedCourse;
                model.SelectedSlot.RoomID = pageviewmodel.FormSelectedRoom;

                if (model.SelectedSlot.SlotID == -1)
                {
                    slotRepository.InsertSlot(model.SelectedSlot);
                    teacherScheduleViewModel = null;
                }
                else
                {
                    slotRepository.UpdateSlot(model.SelectedSlot);
                    teacherScheduleViewModel = null;
                }
                //var b = model.FormSelectedCourse;
                this.ViewBag.selectedSlot = null as Slot;
            }
            return RedirectToAction("Schedule");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Delete")]
        public ActionResult Delete()
        {
            var model = teacherScheduleViewModel;

            if (model.SelectedSlot != null)
            {
                this.slotRepository = new SlotRepository(context);

                if (model.SelectedSlot.SlotID != -1)
                {
                    slotRepository.DeleteSlot(model.SelectedSlot.SlotID);
                    teacherScheduleViewModel = null;
                }
            }


            return RedirectToAction("Schedule");
        }


        public ActionResult SlotClick(Slot clickedSlot)
        {
            teacherScheduleViewModel.SelectedSlot = clickedSlot;
            //this.ViewBag.selectedSlot = clickedSlot as Slot;
            return RedirectToAction("Schedule");
        }

        public ActionResult NextWeek_Click(TeacherScheduleViewModel model) 
        {
            if (teacherScheduleViewModel.SelectedDate != null)
            {
                teacherScheduleViewModel.SelectedDate = teacherScheduleViewModel.SelectedDate.AddDays(7);
                var day = CommonFunctions.CustomConversion.GetFirstDayOfWeekFromDate(teacherScheduleViewModel.SelectedDate);
                
                teacherScheduleViewModel.FirstDayOfWeek = day;
                //teacherScheduleViewModel = model;
                teacherScheduleViewModel.ThisWeekSlots = teacherScheduleViewModel.LoadCalandar(day);

            }

            return RedirectToAction("Schedule");
        }

        public ActionResult PreviousWeek_Click(TeacherScheduleViewModel model)
        {
            if (teacherScheduleViewModel.SelectedDate != null)
            {
                teacherScheduleViewModel.SelectedDate = teacherScheduleViewModel.SelectedDate.AddDays(-7);
                var day = CommonFunctions.CustomConversion.GetFirstDayOfWeekFromDate(teacherScheduleViewModel.SelectedDate);
                
                teacherScheduleViewModel.FirstDayOfWeek = day;
                teacherScheduleViewModel.ThisWeekSlots = teacherScheduleViewModel.LoadCalandar(day);
            }

            return RedirectToAction("Schedule");
        }

        public TeacherController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);
            context = new EFContext();
        }

        public TeacherController(IUserProvider u, IDateProvider d, EFContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;

        }
    }
}
