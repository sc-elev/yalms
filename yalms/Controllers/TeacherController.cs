using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yalms.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Administration()
        {
            // viewmodel: TeacherAdministrationViewModel
            return View();
        }

        public ActionResult Assignment()
        {
            // viewmodel: TeacherAssignmentViewModel
            return View();
        }

        public ActionResult Document()
        {
            // viewmodel: TeacherDocumentViewModel
            return View();
        }

        public ActionResult Schedule()
        {
            // viewmodel: TeacherScheduleViewModel
            return View();
        }
    }
}