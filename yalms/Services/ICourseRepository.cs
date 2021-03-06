using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using yalms.DAL;

namespace yalms.Services 
{ 

    public interface ICourseRepository : IDisposable 
    {
        List<Course> GetAllCoursesBySchoolClassID(int? schoolClassID);
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetAllCoursesByTeacherIDAndWeek_Full(int teacher_UserID, DateTime date);
        IEnumerable<Course> GetAllCoursesByTeacherID_ClassAndAssignment_Full(int teacher_UserID);
        Course GetCourseByID(int? courseID);
        Course GetCourseByClassID(int classID);
        Course GetNewestCourse();
        void InsertCourse(Course company); 
        void DeleteCourse(int courseID); 
        void UpdateCourse(Course company); 
        void Save(); 

     } 
} 

