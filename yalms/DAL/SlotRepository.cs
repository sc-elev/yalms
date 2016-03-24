 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using System;
using yalms.CommonFunctions; 

namespace yalms.DAL
{

 
    public class SlotRepository: ISlotRepository
    {
        // Get context for specific connectionstring.
        private EFContext context;


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
        //#region Get students daily Schedule by date
        //public IEnumerable<Slot> GetStudentsDailySheduleByStudentUserID(int studentUserID, DateTime when)
        //{
        //    return (from slot in context.Slots
        //            join cour in context.Courses on slot.CourseID equals cour.CourseID
        //            join cost in context.Course_Students on cour.CourseID equals cost.CourseID
        //            where cost.Student_UserID == studentUserID && slot.When == when
        //            select slot);
        //}
        //#endregion

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


        #region System functions.
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


        public SlotRepository()
        {
            context = new EFContext();
        }

        public SlotRepository(EFContext ctx)
        {
            context = ctx;
        }


    }
}

