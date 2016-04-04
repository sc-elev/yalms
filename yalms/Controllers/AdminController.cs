using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

namespace yalms.Content.Controllers
{
    public class AdminController : Controller
    {
        protected IDateProvider dateProvider;

        protected IUserProvider userProvider;

        protected EFContext context;
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel(context);
            return View(model);
        }

        public AdminController()
        {
            dateProvider = new DateProvider();
            userProvider = new UserProvider(this);
            context = new EFContext();
        }

        public AdminController(IUserProvider u, IDateProvider d, EFContext c)
        {
            dateProvider = d;
            userProvider = u;
            context = c;
        }
    }
}