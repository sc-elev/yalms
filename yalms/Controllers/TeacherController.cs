using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

// must be present for 
using Microsoft.AspNet.Identity;
using yalms.DAL;
using yalms.CommonFunctions;

namespace yalms.Controllers
{
    public class TeacherController : Controller
    {

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
            var alle = 1;
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

            if (Session["selectedDate"] == null)
            {
                Session["selectedDate"] = dateProvider.Today();
            }
            else
            {
                selectedDate = (DateTime)Session["selectedDate"];
            }


            var teacher_UserID = -1;
            try
            {
                teacher_UserID = this.userProvider.UserID();
            }
            catch { }

            var modelFactory = new TeacherScheduleViewModelFactory(this, context, this.userProvider);
            var model = modelFactory.Create(selectedDate);


            UrlHelper urlHelper;
            if (this.Request != null)
                urlHelper = new UrlHelper(this.Request.RequestContext);
            else
                urlHelper = new UrlHelper();
            if (this.Request != null) model.BuildSlotUrls(urlHelper);

            if (Session != null && Session["selectedSlot"] != null)
            {
                var slot = (Slot)Session["selectedSlot"];
                if (slot.SlotID != -1)
                {
                    try
                    {
                        model.FormSelectedCourse = (int)slot.CourseID;
                    }
                    catch { }

                    try
                    {
                        model.FormSelectedRoom = (int)slot.RoomID;
                    }
                    catch { }
                }

                ViewBag.SelectedSlotInformation = slot.When.ToShortDateString() 
                    + " (" + model.SlotTimings[slot.SlotNR].start.ToLongTimeString().Substring(0, 5)
                    + " - " + model.SlotTimings[slot.SlotNR].end.ToLongTimeString().Substring(0, 5) + ")";
            } else {
                ViewBag.SelectedSlotInformation ="- Ingen vald -";
            }

            return View(model);
        }



        [HttpPost]
        [MultipleButton(Name = "action", Argument ="Save")]
        public ActionResult Save(TeacherScheduleViewModel pageviewmodel)
        {
            if (Session["selectedSlot"] != null && pageviewmodel.FormSelectedCourse != -1 && pageviewmodel.FormSelectedRoom != -1)
            {
                this.slotRepository = new SlotRepository(context);

                var slot = (Slot)Session["selectedSlot"];
                // update from form
                slot.CourseID = pageviewmodel.FormSelectedCourse;
                slot.RoomID = pageviewmodel.FormSelectedRoom;

                if (slot.SlotID == -1)
                {
                    slotRepository.InsertSlot(slot);
                }
                else
                {
                    slotRepository.UpdateSlot(slot);
                }
                //var b = model.FormSelectedCourse;
                Session["selectedSlot"] = null;
            }
            return RedirectToAction("Schedule");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Delete")]
        public ActionResult Delete()
        {
            if (Session["selectedSlot"] != null)
            {
                this.slotRepository = new SlotRepository(context);

                var slot = (Slot)Session["selectedSlot"];
                if (slot.SlotID != -1)
                {
                    slotRepository.DeleteSlot(slot.SlotID);
                }
            }
            return RedirectToAction("Schedule");
        }


        public ActionResult SlotClick(Slot clickedSlot)
        {

            Session["selectedSlot"] = clickedSlot;
            return RedirectToAction("Schedule");
        }

        public ActionResult NextWeek_Click(TeacherScheduleViewModel model) {

            if (Session["selectedDate"] != null)
            {
                var date = (DateTime)Session["selectedDate"];
                Session["selectedDate"] = date.AddDays(7);
            }


            return RedirectToAction("Schedule");
        }

        public ActionResult PreviousWeek_Click(TeacherScheduleViewModel model)
        {
            if (Session["selectedDate"] != null)
            {
                var date = (DateTime)Session["selectedDate"];
                Session["selectedDate"] = date.AddDays(-7);
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
