 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

 
    public class SlotRepository: ISlotRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();


        #region Get all Slots.
        public IEnumerable<Slot> GetAllSlots()
        {
            return context.Slots;
        }

        #endregion

        #region Get students daily Schedule by Student_userID, week,day
        public IEnumerable<Slot> GetStudentsDailySheduleByStudentUserID(int studentUserID, int week, int weekday)
        {
            return (from slot in context.Slots
                    join cour in context.Courses on slot.CourseID equals cour.CourseID
                    join cost in context.Course_Students on cour.CourseID equals cost.CourseID
                    where cost.Student_UserID == studentUserID && slot.WeekNR == week && slot.WeekDay == weekday
                        select slot);
        }
        #endregion

        #region Get students weekly Schedule by Student_userID, week,day
        public IEnumerable<Slot> GetStudentsWeeklySheduleByStudentUserID(int studentUserID, int week)
        {
            return (from slot in context.Slots
                    join cour in context.Courses on slot.CourseID equals cour.CourseID
                    join cost in context.Course_Students on cour.CourseID equals cost.CourseID
                    where cost.Student_UserID == studentUserID && slot.WeekNR == week
                    select slot);
        }
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

        #region Insert new Slot object and register what user created it and when.
        public void InsertSlot(Slot slot, int userID)
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



        #region Update existing Slot object and register what user modified it and when.
        public void UpdateSlot (Slot newSlot,int userID)
        {
            // Get existing Slot object by ID for update.
            var oldSlot = context.Slots.SingleOrDefault(o => o.SlotID == newSlot.SlotID);
            oldSlot.SlotNR = newSlot.SlotNR;
            oldSlot.WeekDay = newSlot.WeekDay;
            oldSlot.WeekNR = newSlot.WeekNR;



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


    }
}

