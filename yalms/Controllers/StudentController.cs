using System;
using System.Collections.Generic;
using System.IO;
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

        private void seedUploads()
        {
            string[] paths = new string[] {
                "~/Upload", "~/Upload/Assignments", "~/Upload/Shared"};
            foreach (var path in paths)
            {
                var dirpath =
                    System.Web.HttpContext.Current.Server.MapPath(path);
                if (Directory.Exists(dirpath)) Directory.Delete(dirpath, true);
                Directory.CreateDirectory(dirpath);
            }
        }

     
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


        [HttpPost]
        public ActionResult PostAssignment(HttpPostedFileBase assignmentFile, 
                                           int assignmentID)
        {
            seedUploads();
            int id = assignmentID;
            StudentMainViewModel model = modelFactory.Create(null);

            if (assignmentFile == null || assignmentFile.ContentLength < 10)
            {
                ViewBag.UploadMessage = "Fel: Ingenting laddades upp";
                return View("MainView", model);
            }
            string [] parts = {
                 "~", "Upload", "Assignments", userProvider.UserID().ToString()};
            var path = Path.Combine(parts);
            path = System.Web.HttpContext.Current.Server.MapPath(path); //FIXME - testability.
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(
                path, assignmentID.ToString() + "-" + assignmentFile.FileName);
            string msg = "Filen " + assignmentFile.FileName + " uppladdad.";
            if (System.IO.File.Exists(path))
            {
                msg += " (Filen fanns sedan förut, raderar.)";
                System.IO.File.Delete(path);
            }
            assignmentFile.SaveAs(path);
            ViewBag.UploadMessage = msg;
            return View("MainView", model);
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
