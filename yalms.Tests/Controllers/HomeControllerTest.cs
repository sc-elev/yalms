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
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 1);
            var context = new Mock<YalmContext>();
            IDateProvider today = new DummyDateProvider(DateTime.Now);

            HomeController controller = 
                new HomeController(today,  who, context.Object);

            // Act
            ActionResult result = controller.Index() as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("yalms | Yet Another Learning Management System", 
                            result.ViewBag.Message);
        }

        [Test]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
