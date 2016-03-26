using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

// must be present for 
using Microsoft.AspNet.Identity;
using yalms.DAL;

namespace yalms.Controllers
{
    public class TeacherController : Controller
    {

        protected IDateProvider dateProvider;
        protected IUserProvider userProvider;
        protected EFContext context;

        // GET: Teacher
        public ViewResult Administration()
        {
            // viewmodel: TeacherAdministrationViewModel
            return View();
        }

        public ViewResult Assignment()
        {
            // viewmodel: TeacherAssignmentViewModel
            return View();
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

            if (Session["selectedDate"] == null)
            {
                Session["selectedDate"] = dateProvider.Today();
            }

            var selectedDate = dateProvider.Today();

            var teacher_UserID = -1;
            try
            {
                teacher_UserID = this.userProvider.UserID();
            }
            catch { }
            //var userID = user.Id;

            TeacherScheduleViewModel model = 
                new TeacherScheduleViewModel((DateTime)Session["selectedDate"], teacher_UserID, context);
            UrlHelper urlHelper = new UrlHelper(this.Request.RequestContext);
            model.BuildSlotUrls(urlHelper);
            if (Session["selectedSlot"] != null)
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
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult SlotForm(TeacherScheduleViewModel pageviewmodel)
        {
            if (Session["selectedSlot"] != null)
            {
                var slot = (Slot)Session["selectedSlot"];
                // update from form
                slot.CourseID = pageviewmodel.FormSelectedCourse;
                slot.RoomID = pageviewmodel.FormSelectedRoom;

                if (slot.SlotID == -1)
                {
                    new SlotRepository().InsertSlot(slot);
                }
                else
                {
                    new SlotRepository().UpdateSlot(slot);
                }
                //var b = model.FormSelectedCourse;
                Session["selectedSlot"] = null;
            }
            return RedirectToAction("Schedule");
        }


        public ActionResult DeleteSelectedSlot()
        {
            if (Session["selectedSlot"] != null)
            {
                var slot = (Slot)Session["selectedSlot"];
                if (slot.SlotID != -1)
                {
                    new SlotRepository().DeleteSlot(slot.SlotID);
                }
            }
            return RedirectToAction("Schedule");
        }


        public ActionResult SlotClick(Slot clickedSlot, TeacherScheduleViewModel model)
        {

            Session["selectedSlot"] = clickedSlot;
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