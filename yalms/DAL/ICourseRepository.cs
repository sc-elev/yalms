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


        Course GetCourseByID(int? courseID); 

        Course GetNewestCourse();
        void InsertCourse(Course company, int courseID); 
        void DeleteCourse(int courseID); 
        void UpdateCourse(Course company, int courseID); 
        void Save(); 

     } 
} 

