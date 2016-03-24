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
        protected Mock<EFContext> GetStandardContext()
        {
            var users = new List<ApplicationUser> { 
                new ApplicationUser("student1"),
                new ApplicationUser("J Edgar Hoover"),
                new ApplicationUser("student3"),
                new ApplicationUser("teacher1"),
                new ApplicationUser("teacher2"),
                new ApplicationUser("user1"),
            };
            var courses = new List<Course> {
                new Course { Name = "kurs1", SchoolClassID = 1, 
                             Teacher_UserID = 1, CourseID = 1},
                new Course { Name = "kurs2", SchoolClassID = 1, 
                             Teacher_UserID = 2, CourseID = 2},
                new Course { Name = "kurs3", SchoolClassID = 2, 
                             Teacher_UserID = 2, CourseID = 3}
                // FIXME: Student_UserID => string
            };
            var classes = new List<SchoolClass> {
                new SchoolClass {Name = "7b", SchoolClassID = 1},
                new SchoolClass {Name = "7c", SchoolClassID = 2},
             };
            var classMembers = new List<SchoolClassStudent> {
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 0},
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 1},
                new SchoolClassStudent { SchoolClassID = 2, Student_UserID = 5},
            };
            var _today = DateTime.Now.Date;
            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(8) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(9) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(10) },
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(12) },
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(13) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(14) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(32) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(33) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(34) },
            };
            var rooms = new List<Room> {
                new Room { RoomID = 1, Description = "E265" }
            };
            var context = new Mock<EFContext>();
            context.Setup(x => x.GetCourses()).Returns(courses);
            context.Setup(x => x.GetSlots()).Returns(slots);
            context.Setup(x => x.GetUsers()).Returns(users);
            context.Setup(x => x.GetSchoolClassStudents()).Returns(classMembers);
            context.Setup(x => x.GetSchoolClasses()).Returns(classes);
            context.Setup(x => x.GetRooms()).Returns(rooms);

            return context;
        }


        [Test]
        public void StudentCtrlReturnsValidDate()
        {
            var context = GetStandardContext();
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who = 
                new DummyUserProvider("J Edgar Hoover", "student",1);
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();

            Assert.AreEqual("26 mar", 
                            ((StudentMainViewModel)action.Model).Date);    
        }


        [Test]
        public void StudentCtrlReturnsValidSlots()
        {
            YalmContext context = GetStandardContext().Object;
            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student",1);
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
                new DummyUserProvider("J Edgar Hoover", "student",1);
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
            var userManager = new UserManager<MemoryUser>(new MemoryUserStore());
            var users = new List<ApplicationUser> { 
                new ApplicationUser("student1"),
                new ApplicationUser("J Edgar Hoover"),
                new ApplicationUser("student3"),
                new ApplicationUser("teacher1"),
                new ApplicationUser("teacher2"),
                new ApplicationUser("user1"),
            };
            var courses = new List<Course> {
                new Course { Name = "kurs1", SchoolClassID = 1, 
                             Teacher_UserID = 1, CourseID = 1},
                new Course { Name = "kurs2", SchoolClassID = 1, 
                             Teacher_UserID = 2, CourseID = 2},
                new Course { Name = "kurs3", SchoolClassID = 2, 
                             Teacher_UserID = 2, CourseID = 3}
            };
            var classes = new List<SchoolClass> {
                new SchoolClass {Name = "7b", SchoolClassID = 1},
                new SchoolClass {Name = "7c", SchoolClassID = 2},
             };
            var classMembers = new List<SchoolClassStudent> {
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 0},
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 1},
                new SchoolClassStudent { SchoolClassID = 2, Student_UserID = 5},
            };
            var _today = DateTime.Now.Date;
            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(8) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(9) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(10) },
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(12) },
           
            };
            var rooms = new List<Room> {
                new Room { RoomID = 1, Description = "E265" }
            };
            var context = new Mock<YalmContext>();
            context.Setup(x => x.GetCourses()).Returns(courses);
            context.Setup(x => x.GetSlots()).Returns(slots);
            context.Setup(x => x.GetUsers()).Returns(users);
            context.Setup(x => x.GetSchoolClassStudents()).Returns(classMembers);
            context.Setup(x => x.GetSchoolClasses()).Returns(classes);
            context.Setup(x => x.GetRooms()).Returns(rooms);

            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student",1);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual("kurs1", model.slots[0].Course.Name);
        }
    }
}
