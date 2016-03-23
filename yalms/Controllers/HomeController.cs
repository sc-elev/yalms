using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using yalms.DAL;
using yalms.Models;



namespace yalms.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        protected IDateProvider dateProvider;
        protected IUserProvider userProvider;


        public ActionResult Index()
        {
            // debugkod
            userProvider = new UserProvider(this);

            // Alle added code to redirect depending un succesfull
            if (userProvider.Role() == "teacher")
            {
                return RedirectToAction("Schedule", "Teacher");
            }
            if (userProvider.Role() == "student")
            {
                return RedirectToAction("MainView", "Student");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "yalms | Yet Another Learning Management System";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "StudentConsulting";

            return View();
        }
    }
}
