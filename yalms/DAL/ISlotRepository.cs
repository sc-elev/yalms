using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 
   /// <summary>
   /// ---------------------------------------------------------------------------------------------------
   /// File Autogenerated at 2016-03-16 23:23:38
   /// Object derived from the  Slot table in the LMSProject database.
   /// Solution version: 1.0.0.1
   /// [ ] Check box with 'x' to prevent entire file from being overwritten. 
   /// Do not remove or change the DMZ region markers. Code inside the DMZ will not be overwritten by auto generation.
   /// ---------------------------------------------------------------------------------------------------
   /// </summary>
    public interface ISlotRepository : IDisposable 
    { 

        IEnumerable<Slot> GetAllSlots(); 
        Slot GetSlot_SimpleByID(int? slotID); 
        Slot GetSlotByID(int? slotID); 

        Slot GetNewestSlot(); 
        void InsertSlot(Slot company, int slotID); 
        void DeleteSlot(int slotID);  
        void UpdateSlot(Slot company, int slotID); 
        void Save(); 


     } 
} 

