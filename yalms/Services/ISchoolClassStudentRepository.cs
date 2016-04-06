using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.Services 
{ 

    public interface ISchoolClassStudentRepository : IDisposable 
    { 
        IEnumerable<SchoolClassStudent> GetAllSchoolClassStudents();
        IEnumerable<SchoolClassStudent> GetAllSchoolClassStudentsBySchoolClassID(int? schoolClassID);
        IEnumerable<SchoolClassStudent> GetAllSchoolClassStudentsBySchoolClassID_Full(int schoolClassID);

        SchoolClassStudent GetSchoolClassStudentByID(int? schoolClassStudentID); 

        SchoolClassStudent GetNewestSchoolClassStudent(); 
        void InsertSchoolClassStudent(SchoolClassStudent company); 
        void DeleteSchoolClassStudent(int schoolClassStudentID); 
        void UpdateSchoolClassStudent(SchoolClassStudent company); 
        void Save(); 


     } 
} 

