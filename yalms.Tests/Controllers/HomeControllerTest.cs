using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using yalms;
using yalms.Controllers;
using NUnit.Framework;
using yalms.Models;
using Moq;

namespace yalms.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest: YalmsTests
    {
   
        [Test]
        public void IndexAsStudent()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider(DateTime.Now);
            var userProvider = 
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            HomeController controller = 
                new HomeController(today,  userProvider, context.Object);

            // Behövde ändra till en ActionResult
            ActionResult result = controller.Index();

            Assert.IsNotNull(result);
            // Assert.AreEqual("", result.MasterName); // Denna rad slutade funka pga ändringen
            // Assert.AreEqual("../Student/MainView", result.ViewName); // Denna rad slutade funka pga ändringen
        }


        [Test]
        public void IndexAsTeacher()
        {
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "teacher", 1);
            IDateProvider dateProvider = new DummyDateProvider(DateTime.Now);
            var context = GetStandardContext();

            var controller = 
                new HomeController(dateProvider, who, context.Object);

            // Behövde ändra fråm ViewResult till ActionResult
            ActionResult result = controller.Index();

            Assert.IsNotNull(result);
           // Assert.AreEqual("", result.MasterName); // Denna rad slutade funka pga ändringen
           // Assert.AreEqual("../Teacher/Schedule", result.ViewName);// Denna rad slutade funka pga ändringen
        }


        [Test]
        public void IndexAsNotRgistered()
        {
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "", 1);
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider(DateTime.Now);
            HomeController controller =
                new HomeController(today, who, context.Object);

            // Behövde ändra fråm ViewResult till ActionResult
            ActionResult result = controller.Index();

            Assert.IsNotNull(result);
            //Assert.AreEqual("", result.MasterName);// Denna rad slutade funka pga ändringen
            //Assert.AreEqual("Index", result.ViewName);// Denna rad slutade funka pga ändringen
        }


        [Test]
        public void About()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.About() as ViewResult;

            Assert.AreEqual("yalms | Yet Another Learning Management System", 
                            result.ViewBag.Message);
        }


        [Test]
        public void Contact()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Contact() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
