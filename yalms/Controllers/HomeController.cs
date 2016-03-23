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
        protected YalmContext context;


        public ActionResult Index()
        {
            // debugkod
            IDateProvider today = new DummyDateProvider(DateTime.Now);

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

        public HomeController() 
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);
            context = new EFContext();
        }

        public HomeController(IDateProvider when, 
                              IUserProvider who, 
                              YalmContext ctx)
        {
            dateProvider = when;
            userProvider = who;
            context = ctx;
        }
    }
}
