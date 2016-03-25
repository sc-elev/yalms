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

        protected EFContext context;

        protected StudentMainViewModelFactory modelFactory;
     
        public ViewResult MainView()
        {
            StudentMainViewModel model = modelFactory.Create(null);
            return View(model);
        }


        [HttpPost]
        public ViewResult MainView(StudentMainViewModel model)
        {
            return View(model);
        }


        public ViewResult MainViewNextDay( )
        {
            StudentMainViewModel model =
                modelFactory.Create(null, 1);
            return View("MainView", model);
        }


        public ViewResult MainViewPrevDay()
        {
            StudentMainViewModel model =
                modelFactory.Create(null, -1);
            return View("MainView", model);
        }


        public ViewResult MainViewToday()
        {
            StudentMainViewModel model =
                modelFactory.Create(dateProvider);
            return View("MainView", model);
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
            modelFactory = new StudentMainViewModelFactory(this, context, userProvider);

        }

        public StudentController(IUserProvider u, IDateProvider d, EFContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;
            modelFactory = new StudentMainViewModelFactory(this, context, userProvider);
        }
    }
}
