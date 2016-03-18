 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;

namespace yalms.DAL
{

    public class Course_StudentRepository: ICourse_StudentRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();


        #region Get all Course_Students
        public IEnumerable<Course_Student> GetAllCourse_Students()
        {
            return context.Course_Students;
        }

        #endregion



        #region Get Course_Student by its Couser_Student ID
        public Course_Student GetCourse_StudentByID(int? couser_StudentID)
        {
            // Get single Couser_Student by its unique ID
            return context.Course_Students.SingleOrDefault(o => o.Course_StudentID == couser_StudentID);

        }
        #endregion

        #region Get newest Course_Student.
        public Course_Student GetNewestCourse_Student()
        {
           return context.Course_Students.OrderByDescending(u => u.Course_StudentID).FirstOrDefault();
        }
        #endregion

        #region Insert new Course_Student object and register what user created it and when.
        public void InsertCourse_Student(Course_Student course_Student, int userID)
        {
            // Add Couser_Student to context
            context.Course_Students.Add(course_Student);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Course_Student  from database by Course_Student ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteCourse_Student (int course_StudentID)
        {
            // Get Couser_Student by ID.
            Course_Student course_Student = context.Course_Students.SingleOrDefault(o => o.Course_StudentID == course_StudentID);
            context.Course_Students.Remove(course_Student);
        }
        #endregion



        #region Update existing Couser_Student object and register what user modified it and when.
        public void UpdateCourse_Student (Course_Student newCouser_Student,int userID)
        {
            // Get existing Course_Student object by ID for update.
            var oldCouser_Student = context.Course_Students.SingleOrDefault(o => o.Course_StudentID == newCouser_Student.Course_StudentID);


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

