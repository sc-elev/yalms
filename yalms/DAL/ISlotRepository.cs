using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface ISlotRepository : IDisposable 
    { 

        IEnumerable<Slot> GetAllSlots(); 

        //IEnumerable<Slot> GetStudentsDailySheduleByStudentUserID(int studentUserID, DateTime when);
        //IEnumerable<Slot> GetStudentsWeeklySheduleByStudentUserID(int studentUserID, DateTime when);
        // FIXME; MOve to ´model(s)

        Slot GetSlotByID(int? slotID); 

        Slot GetNewestSlot(); 
        void InsertSlot(Slot company, int slotID); 
        void DeleteSlot(int slotID);  
        void UpdateSlot(Slot slot, int slotID, DateTime when); 
        void Save(); 


     } 
} 

