using System;
using NUnit.Framework;
using yalms.Tests.Controllers;
using yalms.DAL;
using System.Collections.Generic;
using yalms.Models;

namespace yalms.Tests
{
    [TestFixture]
    public class DAL_tests: YalmsTests
    {

        [Test]
        public void TestStudentsDailySchedule()
        {
            var when = new DummyDateProvider("2016-03-25").Today();
            var ctx = GetStandardContext(when).Object;
            var repo = new SlotRepository(ctx);
            ;

            var result = repo.GetStudentsDailySheduleByStudentUserID(3, when);
            var resultList = new List<Slot>(result);

            Assert.AreNotEqual(null, result);
            Assert.AreEqual(5, resultList.Count);
            Assert.AreNotEqual(null, resultList[0].Course);
            Assert.AreNotEqual(null, resultList[0].Room);
        }
    }
}
