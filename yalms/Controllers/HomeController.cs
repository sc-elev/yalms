﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using yalms.Services;
using yalms.Models;
using yalms.DAL;



namespace yalms.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        protected IDateProvider dateProvider;
        protected IUserProvider userProvider;
        protected EFContext context;


        public ActionResult Index()
        {
            // debugkod
            IDateProvider today = new DummyDateProvider(DateTime.Now);

            // Alle added code to redirect depending un succesfull
            if (userProvider.Role() == "teacher")
            {
                //var model = new TeacherScheduleViewModel(dateProvider.Today(), 
                //                                         userProvider.UserID(),
                //                                         context);
                return RedirectToAction("Schedule","Teacher");
               // return View("../Teacher/Schedule", model);
            }
            if (userProvider.Role() == "student")
            {
                //var alle = new StudentMainViewModel();
                

                var model = new StudentMainViewModel(
                                  context, userProvider, dateProvider);
                TempData["StudentViewModel"] = model;
                //return RedirectToAction("Schedule", "Teacher", model);
                return View("../Student/MainView", model);
            }
            return View("Index");
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
                              EFContext ctx)
        {
            dateProvider = when;
            userProvider = who;
            context = ctx;
        }
    }
}
