using System;
using NUnit.Framework;
using yalms.Models;
using yalms.Controllers;
using Moq;
using System.Web.Mvc;

namespace yalms.Tests.Controllers
{
    [TestFixture]
    public class TeacherCtrlTests: YalmsTests
    {
        //[Test]
        public void TeacherSchedleReturnsValidSchema()
        {

            IDateProvider today = new DummyDateProvider("2016-02-25");
            var context = GetStandardContext(today.Today());
            IUserProvider who =
                new DummyUserProvider("teacher1", "teacher", 1);
            var controller = new TeacherController(who, today, context.Object);

            ViewResult result = controller.Schedule();

            TeacherScheduleViewModel model = (TeacherScheduleViewModel)result.Model;
            Assert.AreEqual(new DummyDateProvider("2016-02-22").Today(), model.FirstDayOfWeek);
            Assert.AreEqual(1, model.ThisWeekSlots[0, 3].CourseID);
            Assert.AreEqual(null, model.ThisWeekSlots[0, 2]);
            Assert.AreEqual(null, model.ThisWeekSlots[0, 1]);
            Assert.AreEqual(null, model.ThisWeekSlots[2, 3]);
            Assert.AreEqual(1, model.ThisWeekSlots[3, 3].CourseID);
            Assert.AreEqual(null, model.ThisWeekSlots[4, 3]);
        }
    }
}
