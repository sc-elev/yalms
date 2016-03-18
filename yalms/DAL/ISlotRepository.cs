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

        IEnumerable<Slot> GetStudentsDailySheduleByStudentUserID(int studentUserID, int week, int weekday);
        IEnumerable<Slot> GetStudentsWeeklySheduleByStudentUserID(int studentUserID, int week);

        Slot GetSlotByID(int? slotID); 

        Slot GetNewestSlot(); 
        void InsertSlot(Slot company, int slotID); 
        void DeleteSlot(int slotID);  
        void UpdateSlot(Slot company, int slotID); 
        void Save(); 


     } 
} 

