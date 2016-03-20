using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using yalms.Models;

namespace yalms.Controllers
{
    public class StudentController : Controller
    {
        protected IDateProvider dateProvider;

        protected IUserProvider userProvider;
     
        public ActionResult MainView()
        {
            StudentMainViewModel model = new StudentMainViewModel();
            model.Date = dateProvider.Today().ToString("yyyy-MM-dd");
            return View(model);
        }

        public StudentController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);

        }

        public StudentController(IUserProvider u, IDateProvider d)
        {
            dateProvider = d;
            userProvider = u;
        }
    }
}