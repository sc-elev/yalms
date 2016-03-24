 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace yalms.DAL
{

    public class CourseRepository: ICourseRepository
    {
        // Get context for specific connectionstring.
        private EFContext context;

        #region Get all Courses.
        public IEnumerable<Course> GetAllCourses()
        {
            return context.GetCourses();
        }
        #endregion

        #region Get all Courses.
        public IEnumerable<Course> GetAllCoursesByTeacherIDAndWeek_Full(int teacher_UserID, DateTime date)
        {

            var courses = (from cour in context.GetCourses()
                           where cour.Teacher_UserID == teacher_UserID   
                           select cour
                           );

            foreach (var course in courses)
            {
                course.SchoolClass = new SchoolClassRepository(context).GetSchoolClassBySchoolClassID_Full(course.SchoolClassID);
                course.Assignments = new AssignmentRepository(context).GetAllAssignmentsByCourseID(course.CourseID);
                course.Slots = new SlotRepository(context).GetTeachersWeeklySheduleByCourseIDAndDate_Full(course.CourseID, date);
            }

            return courses;
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
        public void InsertCourse(Course course)
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

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion


        #region Update existing Course object and register what user modified it and when.
        public void UpdateCourse (Course newCourse)
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

        public CourseRepository()
        {
            context = new EFContext();
        }

        public CourseRepository(EFContext ctx)
        {
            context = ctx;
        }


    }
}

