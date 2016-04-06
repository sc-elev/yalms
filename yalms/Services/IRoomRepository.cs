using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.Services 
{ 

    public interface IRoomRepository : IDisposable 
    { 
        IEnumerable<Room> GetAllRooms(); 
        Room GetRoom_SimpleByID(int? roomID); 
        Room GetRoomByID(int? roomID); 

        Room GetNewestRoom(); 
        void InsertRoom(Room company); 
        void DeleteRoom(int roomID); 
        void UpdateRoom(Room company); 
        void Save(); 


     } 
} 

