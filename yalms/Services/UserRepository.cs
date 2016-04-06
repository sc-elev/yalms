 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using yalms.DAL;

namespace yalms.Services
{

    public class UserRepository: IUserRepository
    {
        private EFContext context;

        public UserRepository()
        {
            context = new EFContext();
        }

        public UserRepository(EFContext context)
        {
            this.context = context;
        }

        public List<ApplicationUser> GetAllSchoolClassStudentsBySchoolClassID(int? schoolClassID)
        {
            
            var result= ( from apus in context.GetUsers()
                           join cost in context.GetSchoolClassStudents() on apus.Id equals cost.Student_UserID
                           join cour in context.GetCourses() on cost.SchoolClassID  equals cour.SchoolClassID
                           where cour.SchoolClassID == schoolClassID
                           select apus
                ).Distinct().ToList();

            return result;

        }


        #region Get all Users 
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return context.Users;
        }

        #endregion

        #region Get User by its User ID without populating foregin key data
        public ApplicationUser GetUser_SimpleByID(int id)
        {
            // Get single User by its unique ID
            return context.Users.SingleOrDefault(o => o.Id == id);

        }
        #endregion

        #region Get User by its User ID
        public ApplicationUser GetUserByID(int id)
        {
            // Get single User by its unique ID
            var user = context.Users.SingleOrDefault(o => o.Id == id);


            return user;
        }
        #endregion

        #region Get newest User.
        public ApplicationUser GetNewestUser()
        {
           return context.Users.OrderByDescending(u => u.CreatedAt).FirstOrDefault();
        }
        #endregion

        #region Insert new User object.
        public void InsertUser(ApplicationUser user)
        {

            // Add User to context
            context.Users.Add(user);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete User  from database by User ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteUser (int id)
        {
            // Get User by ID.
            ApplicationUser user = context.Users.SingleOrDefault(o => o.Id  == id);
            context.Users.Remove(user);
        }
        #endregion


        #region Update existing User object.
        public void UpdateUser(ApplicationUser newUser)
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




    }
}

