 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.Services
{

    public class RoomRepository: IRoomRepository
    {
        private EFContext context;

        public RoomRepository()
        {
            context = new EFContext();
        }

        public RoomRepository(EFContext context)
        {
            this.context = context;
        }


        #region Get all Rooms even those tagged as removed and not yet created.
        public IEnumerable<Room> GetAllRooms()
        {
            return context.GetRooms();
        }

        #endregion

        #region Get Room by its Room ID without populating foregin key data
        public Room GetRoom_SimpleByID(int? roomID)
        {
            // Get single Room by its unique ID
            return context.Rooms.SingleOrDefault(o => o.RoomID == roomID);

        }
        #endregion

        #region Get Room by its Room ID
        public Room GetRoomByID(int? roomID)
        {
            // Get single Room by its unique ID
            var room = context.Rooms.SingleOrDefault(o => o.RoomID == roomID);



            return room;
        }
        #endregion

        #region Get newest Room.
        public Room GetNewestRoom()
        {
           return context.Rooms.OrderByDescending(u => u.RoomID).FirstOrDefault();
        }
        #endregion

        #region Insert new Room object.
        public void InsertRoom(Room room)
        {

            // Add Room to context
            context.Rooms.Add(room);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Room  from database by Room ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteRoom (int roomID)
        {
            // Get Room by ID.
            Room room = context.Rooms.SingleOrDefault(o => o.RoomID == roomID);
            context.Rooms.Remove(room);
        }
        #endregion


        #region Update existing Room object.
        public void UpdateRoom (Room newRoom)
        {
            // Get existing Room object by ID for update.
            var oldRoom = context.Rooms.SingleOrDefault(o => o.RoomID == newRoom.RoomID);
            oldRoom.Description = newRoom.Description;

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

