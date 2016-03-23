﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;
using yalms.Models;

namespace yalms.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "yalms: Yet Another Learning Management System.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Alexander Wåhlin &bull; Michael Kolmodin &bull; Pekka Brännbäck";

            return View();
        }
    }
}
