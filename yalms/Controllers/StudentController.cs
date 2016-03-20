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

        protected YalmContext context;
     
        public ActionResult MainView()
        {
            StudentMainViewModel model = new StudentMainViewModel();
            model.Date = dateProvider.Today().ToString("yyyy-MM-dd");
            var today = DateTime.Now.Date;
            model.slots = context.GetSlots()
                            .Where(s => s.When.Date == dateProvider.Today())
                            .OrderBy(w => w.When)
                            .ToList();
            return View(model);
        }

        public StudentController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);

        }

        public StudentController(IUserProvider u, IDateProvider d, YalmContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;
        }
    }
}