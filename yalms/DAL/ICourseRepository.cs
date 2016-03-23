using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models;
using Microsoft.AspNet.Identity.EntityFramework; 

namespace yalms.DAL 
{ 

    public interface ICourseRepository : IDisposable 
    {

        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetAllCoursesByTeacherIDAndWeek_Full(int teacher_UserID, DateTime date);

        Course GetCourseByID(int? courseID); 

        Course GetNewestCourse();
        void InsertCourse(Course company); 
        void DeleteCourse(int courseID); 
        void UpdateCourse(Course company); 
        void Save(); 

     } 
} 

