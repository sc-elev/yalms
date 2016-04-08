 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using System;
using yalms.CommonFunctions;
using yalms.DAL;

namespace yalms.Services
{

 
    public class SlotRepository:BaseRepository, ISlotRepository
    {
        
        public SlotRepository() : base() {}

        public SlotRepository(EFContext ctx) : base(ctx) { }

        #region Get all Slots.
        public IEnumerable<Slot> GetAllSlots()
        {
            return context.Slots;
        }

        #endregion
        #region Get Teachers weekly course Schedule by date
        public List<Slot> GetTeachersWeeklySheduleByCourseIDAndDate_Full(int courseID, DateTime date)
        {
            var weekNr = CustomConversion.GetWeekFromDate(date);

            //Varning Halvful kod.
            var listOfSlots = (from slot in context.GetSlots()
                                where slot.CourseID == courseID
                               select slot).ToList().Where(o => CustomConversion.GetWeekFromDate(o.When) == weekNr).ToList();

            // remove all thats the wrong week. -- CustomConversion.GetWeekFromDate(slot.When.Date) == weekNr

            var allCourses = new CourseRepository(context).GetAllCourses();
            var allRooms = new RoomRepository(context).GetAllRooms();

            foreach (var slot in listOfSlots)
            {
                slot.Course = allCourses.FirstOrDefault(o => o.CourseID == slot.CourseID);
                slot.Room = allRooms.FirstOrDefault(o => o.RoomID == slot.RoomID);
            }

            return listOfSlots;
        }
        #endregion

        // FIXME: Not used, and referring to not-.existing Course_students table.
        // Get student's daily Schedule by date
        public IEnumerable<Slot> GetStudentsDailySheduleByStudentUserID(int studentUserID, DateTime when)
        {
            var scs = context.GetSchoolClassStudents()
                .Where( s=> s.Student_UserID == studentUserID)
                .SingleOrDefault();
            var courses = context.GetCourses()
                .Where(c => c.SchoolClassID == scs.SchoolClassID)
                .Select(c => c.CourseID)
                .ToList();
            return from slot in context.GetSlots()
                   where courses.Contains(slot.CourseID) && slot.When == when
                   join room in context.GetRooms()
                       on slot.RoomID equals room.RoomID
                   join course in context.GetCourses()
                       on slot.CourseID equals course.CourseID
                   select new Slot {
                       SlotID = slot.SlotID,
                       SlotNR = slot.SlotNR,
                       When = slot.When,
                       Room = room,
                       RoomID = slot.RoomID,
                       Course = course,
                       CourseID = course.CourseID
                   };       
        }

    //    public IEnumerable<Slot> GetDailySlotsByTeacher_UserID(int teacher_UserID, DateTime dailyDate)
    //    {
    //        var list = from slot in context.GetSlots() where slot.CourseID
    //                       join course in context.GetCourses()
    //                   on slot.CourseID equals course.CourseID
    //              // where courses.Contains(slot.CourseID) && slot.When == when


    //                //return context.GetSlots()
    //                //    .Where(o => o.When.Date == dailyDate.Date)
    //                //    .Where(o => courseIDs.Contains((int)o.CourseID));
    //        //       join class_ in context.GetSchoolClasses()
    //         //          on upload.SchoolClassID equals class_.SchoolClassID

    //        //var Alle = (from slots in context.GetSlots()
    //        //            where slots.
    //        //             join course in context.GetCourses() on course.
    //                            //where slots.CourseID == courseID
    //        //var listOfSlots = (from slot in context.GetSlots()
    //        //                   where slot.CourseID == courseID
    //        //                   select slot).ToList();
    //       // var listOfSlots = from slots in context.GetSlots().Join(o =>)

    //                           //join sccl in context.GetSchoolClasses() on sccl.

    ////        var innerJoinQuery =
    ////from category in categories
    ////join prod in products on category.ID equals prod.CategoryID
    ////select new { ProductName = prod.Name, Category = category.Name }; //produces flat sequence

    //            return null;
    //    }

        #region Get students weekly Schedule by Student_userID, week,day
        //public IEnumerable<Slot> GetStudentsWeeklySheduleByStudentUserID(int studentUserID, DateTime when)
        //{
        //    int week = StudentMainViewModel.WeekNrByDate(when);
        //    return (from slot in context.Slots
        //            join cour in context.Courses on slot.CourseID equals cour.CourseID
        //            join cost in context.Course_Students on cour.CourseID equals cost.CourseID
        //            where cost.Student_UserID == studentUserID 
        //                  &&  StudentMainViewModel.WeekNrByDate(slot.When) == week
        //            select slot);
        //}
        // FIXME: Move to model.
        #endregion

        #region Get Slot by its Slot ID
        public Slot GetSlotByID(int? slotID)
        {
            // Get single Slot by its unique ID
            var slot = context.Slots.SingleOrDefault(o => o.SlotID == slotID);


            return slot;
        }
        #endregion

        #region Get newest Slot.
        public Slot GetNewestSlot()
        {
           return context.Slots.OrderByDescending(u => u.SlotID).FirstOrDefault();
        }
        #endregion

        #region Insert new Slot object.
        public void InsertSlot(Slot slot)
        {

            // Add Slot to context
            context.Slots.Add(slot);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Slot  from database by Slot ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteSlot (int slotID)
        {
            // Get Slot by ID.
            Slot slot = context.Slots.SingleOrDefault(o => o.SlotID == slotID);
            context.Slots.Remove(slot);
            context.SaveChanges();
        }
        #endregion


        #region Update existing Slot object.
        public void UpdateSlot (Slot newSlot)
        {
            // Get existing Slot object by ID for update.
            var oldSlot = context.Slots.SingleOrDefault(o => o.SlotID == newSlot.SlotID);
            oldSlot.SlotNR = newSlot.SlotNR;
            oldSlot.When = newSlot.When;

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

    }
}

