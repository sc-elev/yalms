using System;
using NUnit.Framework;
using yalms.Models;
using yalms.Controllers;
using System.Web.Mvc;

namespace yalms.Tests.Controllers
{
    [TestFixture]
    public class StudentControllerTests
    {
        [Test]
        public void StudentCtrlReturnsValidSchema()
        {
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who = 
                new DummyUserProvider("J Edgar Hoover", "student");
            var controller = new StudentController(who, today);

            var action = (ViewResult)controller.MainView();

            Assert.AreEqual("2016-03-26", 
                            ((StudentMainViewModel)action.Model).Date);
                             
        }
    }
}
