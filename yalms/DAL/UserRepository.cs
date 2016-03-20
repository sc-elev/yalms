 
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
        public DomainUser GetUser_SimpleByID(string id)
        {
            // Get single User by its unique ID
            return context.Users.SingleOrDefault(o => o.Id == id);

        }
        #endregion

        #region Get User by its User ID
        public DomainUser GetUserByID(string id)
        {
            // Get single User by its unique ID
            var user = context.Users.SingleOrDefault(o => o.Id == id);


            return user;
        }
        #endregion

        #region Get newest User.
        public DomainUser GetNewestUser()
        {
           return context.Users.OrderByDescending(u => u.CreatedAt).FirstOrDefault();
        }
        #endregion

        #region Insert new User object and register what user created it and when.
        public void InsertUser(DomainUser user, string id)
        {

            // Add User to context
            context.Users.Add(user);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete User  from database by User ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteUser (string id)
        {
            // Get User by ID.
            DomainUser user = context.Users.SingleOrDefault(o => o.Id  == id);
            context.Users.Remove(user);
        }
        #endregion

        #region Tag User as removed, and register what user removed it and when.
        public void RemoveUser(DomainUser newUser, string id)
        {
            // Get User for update
            var oldUser = context.Users.Single(o => o.Id == newUser.Id);


            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update existing User object and register what user modified it and when.
        public void UpdateUser(DomainUser newUser, string id)
        {
            // Get existing User object by ID for update.
            var oldUser = context.Users.SingleOrDefault(o => o.Id == newUser.Id);
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

