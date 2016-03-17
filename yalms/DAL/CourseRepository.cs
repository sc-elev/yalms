 
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
   /// File Autogenerated at 2016-03-16 23:23:37
   /// Object derived from the  Course table in the LMSProject database.
   /// Solution version: 1.0.0.1
   /// [ ] Check box with 'x' to prevent entire file from being overwritten. 
   /// Do not move, remove or change the DMZ region markers. Code inside the DMZ will not be overwritten by auto generation.
   /// ---------------------------------------------------------------------------------------------------
   /// </summary>
    public class CourseRepository: ICourseRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();



        #region Get all Courses even those tagged as removed and not yet created.
        public IEnumerable<Course> GetAllCourses()
        {
            return context.Courses;
        }

        #endregion

        #region Get Course by its Course ID without populating foregin key data
        public Course GetCourse_SimpleByID(int? courseID)
        {
            // Get single Course by its unique ID
            return context.Courses.SingleOrDefault(o => o.CourseID == courseID);

        }
        #endregion

        #region Get Course by its Course ID
        public Course GetCourseByID(int? courseID)
        {
            // Get single Course by its unique ID
            var course = context.Courses.SingleOrDefault(o => o.CourseID == courseID);



            return course;
        }
        #endregion

        #region Get newest Course.
        public Course GetNewestCourse()
        {
           return context.Courses.OrderByDescending(u => u.CourseID).FirstOrDefault();
        }
        #endregion

        #region Insert new Course object and register what user created it and when.
        public void InsertCourse(Course course, int userID)
        {
            // Add Course to context
            context.Courses.Add(course);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Course  from database by Course ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteCourse (int courseID)
        {
            // Get Course by ID.
            Course course = context.Courses.SingleOrDefault(o => o.CourseID == courseID);
            context.Courses.Remove(course);
        }
        #endregion


        #region Update existing Course object and register what user modified it and when.
        public void UpdateCourse (Course newCourse,int userID)
        {
            // Get existing Course object by ID for update.
            var oldCourse = context.Courses.SingleOrDefault(o => o.CourseID == newCourse.CourseID);
            oldCourse.Description = newCourse.Description;
            oldCourse.Name = newCourse.Name;

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

