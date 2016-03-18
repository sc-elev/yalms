using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface ISchoolClassStudentRepository : IDisposable 
    { 
        IEnumerable<SchoolClassStudent> GetAllSchoolClassStudents(); 
        SchoolClassStudent GetSchoolClassStudent_SimpleByID(int? schoolClassStudentID); 
        SchoolClassStudent GetSchoolClassStudentByID(int? schoolClassStudentID); 

        SchoolClassStudent GetNewestSchoolClassStudent(); 
        void InsertSchoolClassStudent(SchoolClassStudent company, int schoolClassStudentID); 
        void DeleteSchoolClassStudent(int schoolClassStudentID); 
        void UpdateSchoolClassStudent(SchoolClassStudent company, int schoolClassStudentID); 
        void Save(); 


     } 
} 

