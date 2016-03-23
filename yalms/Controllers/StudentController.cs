using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using yalms.Models;

namespace yalms.Controllers
{

    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
       
        protected IDateProvider dateProvider;

        protected IUserProvider userProvider;

        protected YalmContext context;
     
        public ViewResult MainView()
        {
            StudentMainViewModel model = 
                TempData["StudentViewModel"] as StudentMainViewModel;
            if (model == null)
            {
                model = new StudentMainViewModel(context,
                                                 userProvider.Who(),
                                                 dateProvider);
            }
            TempData["StudentViewModel"] = model;
            return View(model);
        }

        public ActionResult MainViewNextDay( )
        {
            StudentMainViewModel model =
                 TempData["StudentViewModel"] as StudentMainViewModel;
            var tomorrow =
                new DummyDateProvider(model.Today.AddDays(1));
            var  dateProvider = new DummyDateProvider(tomorrow.Today());
            model = 
                new StudentMainViewModel(context, userProvider.Who(), dateProvider);
            TempData["StudentViewModel"] = model;
            return RedirectToAction("MainView");
        }

        public ActionResult MainViewPrevDay()
        {
            StudentMainViewModel model =
                 TempData["StudentViewModel"] as StudentMainViewModel;
            var yesterday =
                new DummyDateProvider(model.Today.AddDays(-1));
            var dateProvider = new DummyDateProvider(yesterday.Today());
            model =
                new StudentMainViewModel(context, userProvider.Who(), dateProvider);
            TempData["StudentViewModel"] = model;
            return RedirectToAction("MainView");
        }


        public ActionResult MainViewToday()
        {
            var model =
                new StudentMainViewModel(context, userProvider.Who(), dateProvider);
            TempData["StudentViewModel"] = model;
            return RedirectToAction("MainView");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) context.Dispose();
            base.Dispose(disposing);
        }

        public StudentController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);
            context = new EFContext();
        }

        public StudentController(IUserProvider u, IDateProvider d, YalmContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;
        }
    }
}
