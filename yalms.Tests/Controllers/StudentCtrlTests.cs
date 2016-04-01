using System;
using NUnit.Framework;
using yalms.Models;
using yalms.Controllers;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using yalms;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace yalms.Tests.Controllers
{


    [TestFixture]
    public class StudentControllerTests: YalmsTests
    {


        [Test]
        public void StudentCtrlReturnsValidDate()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var controller = new StudentController(who, today, context.Object);
            var modelFactory =
                new StudentMainViewModelFactory(controller, context.Object, who);
            var model = modelFactory.Create(today);
            controller.TempData["studentViewModel"] = model;

            var action = (ViewResult)controller.MainView();

            Assert.AreEqual("26 mar",
                            ((StudentMainViewModel)action.Model).Date);
        }

        [Test]
        public void StudentCtrlReturnsNextDay()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var controller = new StudentController(who, today, context.Object);
            var modelFactory =
                new StudentMainViewModelFactory(controller, context.Object, who);
            var model = modelFactory.Create(today);
            controller.TempData["studentViewModel"] = model;

            var action = (ViewResult)controller.MainViewNextDay();

            Assert.AreEqual("27 mar",
                            ((StudentMainViewModel)action.Model).Date);
        }

        [Test]
        public void StudentCtrlReturnsPrevDay()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var controller = new StudentController(who, today, context.Object);
            var modelFactory =
                new StudentMainViewModelFactory(controller, context.Object, who);
            var model = modelFactory.Create(today);
            controller.TempData["studentViewModel"] = model;

            var action = (ViewResult)controller.MainViewPrevDay();

            Assert.AreEqual("25 mar",
                            ((StudentMainViewModel)action.Model).Date);
        }


        [Test]
        public void StudentCtrlReturnsToday()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider("2016-02-26");
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var controller = new StudentController(who, today, context.Object);
            var modelFactory =
                new StudentMainViewModelFactory(controller, context.Object, who);
            var model = modelFactory.Create(today);
            controller.TempData["studentViewModel"] = model;

            var action = (ViewResult)controller.MainViewToday();

            Assert.AreEqual("26 feb",
                            ((StudentMainViewModel)action.Model).Date);
        }


        [Test]
        public void StudentCtrlReturnsValidSlots()
        {
            EFContext context = GetStandardContext().Object;
            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual(5, model.slots.Count());
        }


        [Test]
        public void StudentCtrlReturnsEmptySlotList()
        {
            var context = GetStandardContext();

            var _today = DateTime.Now.Date;
            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, When = _today.AddHours(28) },
                new Slot {CourseID = 2, RoomID = 1, When = _today.AddHours(29) },
                new Slot {CourseID = 3, RoomID = 1, When = _today.AddHours(210) },
                new Slot {CourseID = 1, RoomID = 1, When = _today.AddHours(212) },
                new Slot {CourseID = 1, RoomID = 1, When = _today.AddHours(213) },
                new Slot {CourseID = 2, RoomID = 1, When = _today.AddHours(214) },
                new Slot {CourseID = 2, RoomID = 1, When = _today.AddHours(232) },
                new Slot {CourseID = 3, RoomID = 1, When = _today.AddHours(233) },
                new Slot {CourseID = 3, RoomID = 1, When = _today.AddHours(234) },
            };
            context.Setup(x => x.GetSlots()).Returns(slots);

            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual(new List<Slot>(), model.slots);
        }


        [Test]
        public void StudentCtrlReturnsJoinedAttributes()
        {
            var context = GetStandardContext();

            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual("kurs1", model.slots[0].Course.Name);
        }


        [Test]
        public void StudentCtrlReturnsAssignmentStatus()
        {
            var context = GetStandardContext();

            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student", 4);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual(1, model.SubmissiontStates.Children[0].Children[0].Submissions.Count);
            //FIXME Assert.AreEqual(1, model.SubmissiontStates.Children[1].Children[1].Submissions.Count);
        }
    }
}
