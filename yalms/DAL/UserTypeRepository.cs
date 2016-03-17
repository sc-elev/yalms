 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

   /// <summary>
   /// ---------------------------------------------------------------------------------------------------
   /// File Autogenerated at 2016-03-16 23:23:38
   /// Object derived from the  UserType table in the LMSProject database.
   /// Solution version: 1.0.0.1
   /// [ ] Check box with 'x' to prevent entire file from being overwritten. 
   /// Do not move, remove or change the DMZ region markers. Code inside the DMZ will not be overwritten by auto generation.
   /// ---------------------------------------------------------------------------------------------------
   /// </summary>
    public class UserTypeRepository: IUserTypeRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext(ConfigurationManager.ConnectionStrings["EFContext"].ConnectionString);


        #region Get all UserTypes
        public IEnumerable<UserType> GetAllUserTypes()
        {
            return context.UserTypes;
        }

        #endregion

        #region Get UserType by its UserType ID without populating foregin key data
        public UserType GetUserType_SimpleByID(int? userTypeID)
        {
            // Get single UserType by its unique ID
            return context.UserTypes.SingleOrDefault(o => o.UserTypeID == userTypeID);

        }
        #endregion

        #region Get UserType by its UserType ID
        public UserType GetUserTypeByID(int? userTypeID)
        {
            // Get single UserType by its unique ID
            var userType = context.UserTypes.SingleOrDefault(o => o.UserTypeID == userTypeID);


            return userType;
        }
        #endregion

        #region Get newest UserType.
        public UserType GetNewestUserType()
        {
           return context.UserTypes.OrderByDescending(u => u.UserTypeID).FirstOrDefault();
        }
        #endregion

        #region Insert new UserType object and register what user created it and when.
        public void InsertUserType(UserType userType, int userID)
        {

            // Add UserType to context
            context.UserTypes.Add(userType);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete UserType  from database by UserType ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteUserType (int userTypeID)
        {
            // Get UserType by ID.
            UserType userType = context.UserTypes.SingleOrDefault(o => o.UserTypeID == userTypeID);
            context.UserTypes.Remove(userType);
        }
        #endregion

        #region Tag UserType as removed, and register what user removed it and when.
        public void RemoveUserType(UserType newUserType, int userID)
        {
            // Get UserType for update
            var oldUserType = context.UserTypes.Single(o => o.UserTypeID == newUserType.UserTypeID);


            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update existing UserType object and register what user modified it and when.
        public void UpdateUserType (UserType newUserType,int userID)
        {
            // Get existing UserType object by ID for update.
            var oldUserType = context.UserTypes.SingleOrDefault(o => o.UserTypeID == newUserType.UserTypeID);
            oldUserType.Name = newUserType.Name;


            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update UserType with foreignkey names for presentation.
        private UserType PopulateUserTypeWithForeignKeyDataObjects(UserType userType)
        {
            // Get objects for Sub keys
            return userType;
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

