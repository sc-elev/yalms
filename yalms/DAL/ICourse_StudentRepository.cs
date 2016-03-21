using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface ICourse_StudentRepository : IDisposable 
    { 

        IEnumerable<Course_Student> GetAllCourse_Students();
        Course_Student GetCourse_StudentByID(int? couser_StudentID); 

        Course_Student GetNewestCourse_Student(); 
        void InsertCourse_Student(Course_Student company); 
        void DeleteCourse_Student(int couser_StudentID); 
        void UpdateCourse_Student(Course_Student company); 
        void Save(); 


     } 
} 

