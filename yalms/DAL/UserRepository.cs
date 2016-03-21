 
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

        public List<ApplicationUser> GetAllSchoolClassStudentsBySchoolClassID(int? schoolClassID)
        {

            return (from apus in context.Users
                           join cost in context.Course_Students on apus.Id equals cost.Student_UserID
                           join cour in context.Courses on cost.CourseID equals cour.CourseID
                           where cour.SchoolClassID == schoolClassID
                           select apus
                
                ).ToList();

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
