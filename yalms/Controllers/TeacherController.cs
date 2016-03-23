using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

// must be present for 
using Microsoft.AspNet.Identity;

namespace yalms.Controllers
{
    public class TeacherController : Controller
    {

        protected IDateProvider dateProvider;
        protected IUserProvider userProvider;
        protected YalmContext context;

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
                Session["selectedDate"] = DateTime.Now;
            }

            var selectedDate = DateTime.Now;

            var teacher_UserID = -1;
            try
            {
                teacher_UserID = this.userProvider.UserID();
            }
            catch { }
            //var userID = user.Id;

            TeacherScheduleViewModel model = new TeacherScheduleViewModel((DateTime)Session["selectedDate"], teacher_UserID);

            return View(model);
        }

        public TeacherController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);
            context = new EFContext();
        }

        public TeacherController(IUserProvider u, IDateProvider d, YalmContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;
        }
    }
}