 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

    public class UserRepository: IUserRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();


        #region Get all Users 
        public IEnumerable<DomainUser> GetAllUsers()
        {
            return context.Users;
        }

        #endregion

        #region Get User by its User ID without populating foregin key data
        public DomainUser GetUser_SimpleByID(int? userID)
        {
            // Get single User by its unique ID
            return context.Users.SingleOrDefault(o => o.UserID == userID);

        }
        #endregion

        #region Get User by its User ID
        public DomainUser GetUserByID(int? userID)
        {
            // Get single User by its unique ID
            var user = context.Users.SingleOrDefault(o => o.UserID == userID);


            return user;
        }
        #endregion

        #region Get newest User.
        public DomainUser GetNewestUser()
        {
           return context.Users.OrderByDescending(u => u.UserID).FirstOrDefault();
        }
        #endregion

        #region Insert new User object and register what user created it and when.
        public void InsertUser(DomainUser user, int userID)
        {

            // Add User to context
            context.Users.Add(user);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete User  from database by User ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteUser (int userID)
        {
            // Get User by ID.
            DomainUser user = context.Users.SingleOrDefault(o => o.UserID == userID);
            context.Users.Remove(user);
        }
        #endregion

        #region Tag User as removed, and register what user removed it and when.
        public void RemoveUser(DomainUser newUser, int userID)
        {
            // Get User for update
            var oldUser = context.Users.Single(o => o.UserID == newUser.UserID);


            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update existing User object and register what user modified it and when.
        public void UpdateUser(DomainUser newUser, int userID)
        {
            // Get existing User object by ID for update.
            var oldUser = context.Users.SingleOrDefault(o => o.UserID == newUser.UserID);
            //oldUser.FullName = newUser.FullName;
            //oldUser.PassWord = newUser.PassWord;
            //oldUser.Title = newUser.Title;
            oldUser.UserName = newUser.UserName;

            // If present, only update foreignkey fields if valid selection has been made.


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

